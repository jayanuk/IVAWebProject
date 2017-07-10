using Core.Log;
using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.FindExpert.Helpers;
using IVA.FindExpert.Helpers.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class MessageController : BaseController
    {

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetThreadById(long ThreadId)
        {
            MessageThreadModel thread = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var t = new MessageThreadRepository(context).GetById(ThreadId);
                    if(t != null)
                    {
                        thread = new MessageThreadModel
                        {
                            Id = t.Id,
                            AgentId = t.AgentId,
                            BuyerId = t.BuyerId,
                            RequestId = t.RequestId,
                            Date = t.CreatedTime.GetAdjustedTime().ToString("dd/MMM"),
                            Time = t.CreatedTime.GetAdjustedTime().ToString("HH:mm"),
                            Messages = t.Messages.OrderBy(
                            m => m.Time).Select(m => new MessageModel
                            {
                                Id = m.Id,
                                ThreadId = m.ThreadId,
                                RecieverId = m.RecieverId,
                                SenderId = m.SenderId,
                                MessageText = m.MessageText,
                                Status = m.Status,
                                QuotationId = m.QuotationId ?? 0,
                                Time = m.Time.GetAdjustedTime().ToString("yyyy-MM-dd hh:mm tt")
                            }).ToList()

                        };

                        var userRepo = new UserRepository(context);
                        var userProfileRepo = new UserProfileRepository(context);
                        var buyer = userRepo.GetByUserId(thread.BuyerId);
                        var agent = userRepo.GetByUserId(thread.AgentId);
                        var buyerProfile = userProfileRepo.GetByUserId(thread.BuyerId);
                        var agentProfile = userProfileRepo.GetByUserId(thread.AgentId);
                        var request = new ServiceRequestRepository(context).GetById(thread.RequestId);
                        thread.AgentName = agent.Name;
                        if (agentProfile != null)
                            thread.AgentName = agentProfile.FirstName + " " + agentProfile.LastName;
                        thread.CompanyName = agent.Company.Name;
                        thread.BuyerName = buyer.Name;
                        if (buyerProfile != null)
                            thread.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;

                        thread.Description = "Vehicle No: " + request.VehicleNo + " / Request: " + request.Code;
                        thread.UnreadMessageCount = thread.Messages.Count(
                            m => m.Status == (int)Constant.MessageStatus.Initial);

                        foreach (var message in thread.Messages)
                        {
                            var sender = userRepo.GetByUserId(message.SenderId);
                            message.SenderName = sender.Name;
                            var senderProfile = userProfileRepo.GetByUserId(message.SenderId);
                            if (senderProfile != null)
                                message.SenderName = senderProfile.FirstName + " " + senderProfile.LastName;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(thread);
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetThreadById(long ThreadId, long UserId)
        {
            MessageThreadModel thread = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var t = new MessageThreadRepository(context).GetById(ThreadId);
                    if (t != null)
                    {
                        thread = new MessageThreadModel
                        {
                            Id = t.Id,
                            AgentId = t.AgentId,
                            BuyerId = t.BuyerId,
                            RequestId = t.RequestId,
                            Date = t.CreatedTime.GetAdjustedTime().ToString("dd/MMM"),
                            Time = t.CreatedTime.GetAdjustedTime().ToString("HH:mm"),
                            Messages = t.Messages.OrderBy(
                            m => m.Time).Select(m => new MessageModel
                            {
                                Id = m.Id,
                                ThreadId = m.ThreadId,
                                RecieverId = m.RecieverId,
                                SenderId = m.SenderId,
                                MessageText = m.MessageText,
                                Status = m.Status,
                                QuotationId = m.QuotationId ?? 0,
                                Time = m.Time.GetAdjustedTime().ToString("yyyy-MM-dd hh:mm tt")
                            }).ToList()

                        };

                        var userRepo = new UserRepository(context);
                        var userProfileRepo = new UserProfileRepository(context);
                        var buyer = userRepo.GetByUserId(thread.BuyerId);
                        var agent = userRepo.GetByUserId(thread.AgentId);
                        var buyerProfile = userProfileRepo.GetByUserId(thread.BuyerId);
                        var agentProfile = userProfileRepo.GetByUserId(thread.AgentId);
                        var request = new ServiceRequestRepository(context).GetById(thread.RequestId);

                        thread.AgentName = agent.Name;
                        if (agentProfile != null)
                            thread.AgentName = agentProfile.FirstName + " " + agentProfile.LastName;
                        thread.CompanyName = agent.Company.Name;
                        thread.BuyerName = buyer.Name;
                        if (buyerProfile != null)
                            thread.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;

                        thread.Description = "Vehicle No: " + request.VehicleNo + " / Request: " + request.Code;
                        thread.RequestStatus = request.Status;

                        var latestMessageWithQuote = thread.Messages.Where(
                            m => m.QuotationId > 0).OrderByDescending(m => m.Id).FirstOrDefault()?.Id;

                        thread.UnreadMessageCount = thread.Messages.Count(
                            m => m.Status == (int)Constant.MessageStatus.Initial && m.RecieverId == UserId);
                        
                        foreach (var message in thread.Messages)
                        {
                            var sender = userRepo.GetByUserId(message.SenderId);
                            message.SenderName = sender.Name;
                            var senderProfile = userProfileRepo.GetByUserId(message.SenderId);
                            if (senderProfile != null)
                                message.SenderName = senderProfile.FirstName + " " + senderProfile.LastName;
                            if (message.QuotationId > 0)
                                thread.HasQuotation = true;

                            if (message.Id != latestMessageWithQuote)
                                message.QuotationId = 0;
                            else
                                message.MessageText = "New " + message.MessageText;
                        }

                        new MessageRepository(context).UpdateMessgesToRead(UserId, ThreadId);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(thread);
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetBuyerThreads(long UserId, int Page)
        {
            List<MessageThreadModel> threads = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var userThreads = new MessageThreadRepository(context).GetByBuyerId(UserId);
                    userThreads = userThreads.Skip((Page - 1) * Constant.Paging.MESSAGE_THREADS_PER_PAGE).
                        Take(Constant.Paging.MESSAGE_THREADS_PER_PAGE).ToList();
                    threads = userThreads.Select(t => new MessageThreadModel
                    {
                        Id = t.Id,
                        AgentId = t.AgentId,
                        BuyerId = t.BuyerId,  
                        RequestId = t.RequestId,                      
                        Date = t.CreatedTime.GetAdjustedTime().ToString("dd/MMM"),
                        Time = t.CreatedTime.GetAdjustedTime().ToString("HH:mm"),
                        Messages = t.Messages.OrderBy(
                            m => m.Time).Select(m => new MessageModel
                            {
                                Id = m.Id,
                                ThreadId = m.ThreadId,
                                RecieverId = m.RecieverId,
                                SenderId = m.SenderId,                                
                                MessageText = m.MessageText,
                                Status = m.Status,
                                QuotationId = m.QuotationId ?? 0,
                                Time = m.Time.GetAdjustedTime().ToString("yyyy-MM-dd HH:mm")
                            }).ToList()

                    }).ToList();

                    foreach(var thread in threads)
                    {
                        var userRepo = new UserRepository(context);
                        var userProfileRepo = new UserProfileRepository(context);
                        var buyer = userRepo.GetByUserId(thread.BuyerId);
                        var agent = userRepo.GetByUserId(thread.AgentId);
                        var buyerProfile = userProfileRepo.GetByUserId(thread.BuyerId);
                        var agentProfile = userProfileRepo.GetByUserId(thread.AgentId);
                        var request = new ServiceRequestRepository(context).GetById(thread.RequestId);
                        thread.AgentName = agent.Name;
                        if (agentProfile != null)
                            thread.AgentName = agentProfile.FirstName + " " + agentProfile.LastName;
                        thread.CompanyName = agent.Company.Name;
                        thread.BuyerName = buyer.Name;
                        if (buyerProfile != null)
                            thread.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;

                        thread.Description = "Vehicle No: " + request.VehicleNo + " / Request: " + request.Code;
                        thread.VehicleNo = request.VehicleNo;
                        thread.UnreadMessageCount = thread.Messages.Count(
                            m => m.Status == (int)Constant.MessageStatus.Initial && m.RecieverId == UserId);

                        foreach (var message in thread.Messages)
                        {
                            var sender = userRepo.GetByUserId(message.SenderId);
                            message.SenderName = sender.Name;
                            var senderProfile = userProfileRepo.GetByUserId(message.SenderId);
                            if (senderProfile != null)
                                message.SenderName = senderProfile.FirstName + " " + senderProfile.LastName;
                        }                        
                    }
                    
                    if(Page == 1)
                    {
                        var promotion = new PromotionRepository(context).GetLatestPromotion(1); //TODO change the type to param
                        PromotionModel promModel = null;

                        if (promotion != null)
                        {
                            promModel = new PromotionModel
                            {
                                Id = promotion.Id,
                                Title = promotion.Title,
                                Header = promotion.Header,
                                Description = promotion.Description,
                                CreatedDate = promotion.CreatedDate?.ToString(Constant.DateFormatType.YYYYMMDD),
                                Status = promotion.Status ?? 0,
                                Type = ((promotion.Type ?? 0) == Constant.PromotionType.OFFER) ? "Offers" : "Promotions"
                            };
                            MessageThreadModel promotionEntry = new MessageThreadModel();
                            promotionEntry.Description = promModel.Type;
                            promotionEntry.Promotion = promModel;
                            if (threads == null)
                                threads = new List<MessageThreadModel>();
                            threads.Insert(0, promotionEntry);
                        }
                    }                    
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(threads);
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetAgentThreads(long UserId, int Page)
        {
            List<MessageThreadModel> threads = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var userThreads = new MessageThreadRepository(context).GetByAgentId(UserId);
                    userThreads = userThreads.Skip((Page - 1) * Constant.Paging.MESSAGE_THREADS_PER_PAGE).
                        Take(Constant.Paging.MESSAGE_THREADS_PER_PAGE).ToList();
                    threads = userThreads.Select(t => new MessageThreadModel
                    {
                        Id = t.Id,
                        AgentId = t.AgentId,
                        BuyerId = t.BuyerId,
                        RequestId = t.RequestId,
                        Date = t.CreatedTime.GetAdjustedTime().ToString("dd/MMM"),
                        Time = t.CreatedTime.GetAdjustedTime().ToString("HH:mm"),
                        Messages = t.Messages.OrderBy(
                            m => m.Time).Select(m => new MessageModel
                            {
                                Id = m.Id,
                                ThreadId = m.ThreadId,
                                RecieverId = m.RecieverId,
                                SenderId = m.SenderId,
                                MessageText = m.MessageText,
                                Status = m.Status,
                                QuotationId = m.QuotationId ?? 0,
                                Time = m.Time.GetAdjustedTime().ToString("yyyy-MM-dd HH:mm")
                            }).ToList()

                    }).ToList();

                    foreach (var thread in threads)
                    {
                        var userRepo = new UserRepository(context);
                        var userProfileRepo = new UserProfileRepository(context);
                        var buyer = userRepo.GetByUserId(thread.BuyerId);
                        var agent = userRepo.GetByUserId(thread.AgentId);
                        var buyerProfile = userProfileRepo.GetByUserId(thread.BuyerId);
                        var agentProfile = userProfileRepo.GetByUserId(thread.AgentId);
                        var request = new ServiceRequestRepository(context).GetById(thread.RequestId);
                        thread.AgentName = agent.Name;
                        if (agentProfile != null)
                            thread.AgentName = agentProfile.FirstName + " " + agentProfile.LastName;
                        thread.CompanyName = agent.Company.Name;
                        thread.BuyerName = buyer.Name;
                        if (buyerProfile != null)
                            thread.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;

                        thread.Description = "Vehicle No: " + request.VehicleNo + " / Request: " + request.Code;
                        thread.VehicleNo = request.VehicleNo;
                        thread.UnreadMessageCount = thread.Messages.Count(
                            m => m.Status == (int)Constant.MessageStatus.Initial && m.RecieverId == UserId);

                        foreach (var message in thread.Messages)
                        {
                            var sender = userRepo.GetByUserId(message.SenderId);
                            message.SenderName = sender.Name;
                            var senderProfile = userProfileRepo.GetByUserId(message.SenderId);
                            if (senderProfile != null)
                                message.SenderName = senderProfile.FirstName + " " + senderProfile.LastName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(threads);
        }

        [HttpPost]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult AddThread(MessageThreadModel Model)
        {
            long id = 0;
            try
            {
                MessageThread thread = new MessageThread();
                thread.AgentId = Model.AgentId;
                thread.BuyerId = Model.BuyerId;
                thread.RequestId = Model.RequestId;
                thread.CreatedBy = Model.CreatedBy;
                thread.CreatedTime = DateTime.Now.ToUniversalTime();

                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new MessageThreadRepository(context);
                    id = repo.SafeAdd(thread);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(id);            
        }

        [HttpPost]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult AddMessage(MessageModel Model)
        {
            long Id = 0;
            try
            {
                Message message = new Message
                {
                    Id = Model.Id,
                    ThreadId = Model.ThreadId,
                    MessageText = Model.MessageText,
                    SenderId = Model.SenderId,
                    RecieverId = Model.RecieverId,
                    Status = (int)Constant.MessageStatus.Initial,
                    Time = DateTime.Now.ToUniversalTime()
                };

                using (AppDBContext context = new AppDBContext())
                {
                    var userRepo = new UserRepository(context);
                    var sender = userRepo.GetByUserId(message.SenderId);
                    var recipient = userRepo.GetByUserId(message.RecieverId);

                    if (message.ThreadId == 0)
                    {   
                        long agentId = 0;
                        long buyerId = 0;
                        if (sender.UserType == Constant.UserType.BUYER)
                        {
                            buyerId = sender.Id;
                            agentId = recipient.Id;
                        }
                        else
                        {
                            buyerId = recipient.Id;
                            agentId = sender.Id;
                            new AgentServiceRequestRepository(context).UpdateResponseTime(Model.RequestId, agentId);
                        }

                        var existingthread =
                            new MessageThreadRepository(context).GetByAgentId(agentId).Where(
                                t => t.RequestId == Model.RequestId && t.BuyerId == buyerId).FirstOrDefault();

                        if(existingthread == null)
                        {
                            MessageThread thread = new MessageThread();
                            thread.AgentId = agentId;
                            thread.BuyerId = buyerId;
                            thread.RequestId = Model.RequestId;
                            thread.CreatedBy = Model.SenderId;
                            thread.CreatedTime = DateTime.Now.ToUniversalTime();

                            var repo = new MessageThreadRepository(context);
                            long tid = repo.Add(thread);
                            message.ThreadId = tid;
                        }
                        else
                        {
                            message.ThreadId = existingthread.Id;
                        }  
                    }

                    if (sender.UserType == Constant.UserType.BUYER)
                    {
                        new ServiceRequestRepository(context).UpdateBuyerResponded(Model.RequestId);
                    }

                    Id = new MessageRepository(context).Add(message);
                    new NotificationRepository(context).Add(
                           recipient.Id,
                           (int)Constant.NotificationType.Message,
                           message.ThreadId, ConfigurationHelper.NOTIFICATION_TITLE,
                           Constant.Notification.NEW_MESSAGE_TEXT);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                InternalServerError(ex);
            }

            return Ok(Id);
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult UnreadMessageCount(long UserId)
        {
            long count = 0;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    count = new MessageRepository(context).UnreadMessageCount(UserId);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(MessageController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }

            return Ok(count);
        }
    }
}
