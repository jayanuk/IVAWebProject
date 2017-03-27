using Core.Log;
using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class QuotationController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetById(long Id)
        {
            RequestQuotationViewModel model = null;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var quote = new RequestQuotationRepository(context).GetById(Id);
                    if (quote != null)
                    {
                        model = new RequestQuotationViewModel
                        {
                            Id = quote.Id,
                            ServiceRequestId = quote.ServiceRequestId,
                            QuotationTemplateId = quote.QuotationTemplateId,
                            Premimum = quote.Premimum?.ToString("#,##0.00"),
                            Cover = quote.Cover?.ToString("#,##0.00"),
                            AgentId = quote.AgentId,
                            AgentName = quote.Agent?.Name,
                            AgentContact = quote.Agent?.UserName,
                            CompanyId = quote.Agent?.CompanyId ?? 0,
                            CompanyName = quote.Agent?.Company?.Name,
                            QuotationTemplateName = quote.QuotationTemplate.Name,
                            QuotationText = quote.QuotationText,
                            Status = quote.Status ?? 0,
                            ServiceRequestCode = quote.ServiceRequest.Code,
                            ClaimType = quote.ServiceRequest.ClaimType,
                            VehicleNo = quote.ServiceRequest.VehicleNo,
                            VehicleValue = quote.ServiceRequest.VehicleValue,
                            IsExpired = quote.IsExpired ?? false
                        };

                        if (quote.ServiceRequest.Status == (int)Constant.ServiceRequestStatus.Closed ||
                            quote.ServiceRequest.Status == (int)Constant.ServiceRequestStatus.Expired)
                            model.Status = (int)Constant.QuotationStatus.Closed;

                        var messageThread = new MessageThreadRepository(context).GetByAgentAndRequest(model.AgentId, model.ServiceRequestId);
                        if (messageThread != null)
                            model.ThreadId = messageThread.Id;
                    }
                }
                return Ok(model);
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(QuotationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetByIdFromBuyer(long Id)
        {
            RequestQuotationViewModel model = null;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var quote = new RequestQuotationRepository(context).GetById(Id);
                    if (quote != null)
                    {
                        model = new RequestQuotationViewModel
                        {
                            Id = quote.Id,
                            ServiceRequestId = quote.ServiceRequestId,
                            QuotationTemplateId = quote.QuotationTemplateId,
                            Premimum = quote.Premimum?.ToString("#,##0.00"),
                            Cover = quote.Cover?.ToString("#,##0.00"),
                            AgentId = quote.AgentId,
                            AgentName = quote.Agent?.Name,
                            AgentContact = quote.Agent?.UserName,
                            CompanyId = quote.Agent?.CompanyId ?? 0,
                            CompanyName = quote.Agent?.Company?.Name,
                            QuotationTemplateName = quote.QuotationTemplate.Name,
                            QuotationText = quote.QuotationText,
                            Status = quote.Status ?? 0,
                            ServiceRequestCode = quote.ServiceRequest.Code,
                            ClaimType = quote.ServiceRequest.ClaimType,
                            VehicleNo = quote.ServiceRequest.VehicleNo,
                            VehicleValue = quote.ServiceRequest.VehicleValue,
                            IsExpired = quote.IsExpired ?? false
                        };

                        if (quote.ServiceRequest.Status == (int)Constant.ServiceRequestStatus.Closed ||
                            quote.ServiceRequest.Status == (int)Constant.ServiceRequestStatus.Expired)
                            model.Status = (int)Constant.QuotationStatus.Closed;

                        var messageThread = new MessageThreadRepository(context).GetByAgentAndRequest(model.AgentId, model.ServiceRequestId);
                        if (messageThread != null)
                            model.ThreadId = messageThread.Id;

                        new RequestQuotationRepository(context).UpdateToChecked(quote.Id);
                    }
                }
                return Ok(model);
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(QuotationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Accept(long QuotationId)
        {
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new RequestQuotationRepository(context);
                    var quote = repo.GetById(QuotationId);
                    quote.Status = (int)Constant.QuotationStatus.Accepted;
                    repo.Update(quote);
                    var otherQuotes = repo.GetByRequest(quote.ServiceRequestId)?.Where(q => q.Id != quote.Id);
                    if (otherQuotes != null)
                    {
                        foreach (var q in otherQuotes)
                        {
                            q.Status = (int)Constant.QuotationStatus.Closed;
                            repo.Update(q);
                        }
                    }

                    var reqRepo = new ServiceRequestRepository(context);
                    var userRepo = new UserRepository(context);
                    var request = quote.ServiceRequest;
                    var buyerName = userRepo.GetName(request.UserId);
                    request.Status = (int)Constant.ServiceRequestStatus.Closed;
                    reqRepo.Update(request);

                    MessageModel message = new MessageModel
                    {
                        MessageText = "Quotation accepted by: " + buyerName,
                        RequestId = request.Id,
                        SenderId = request.UserId,
                        RecieverId = quote.AgentId,
                        QuotationId = 0
                    };
                    AddMessage(message, context);

                    new NotificationRepository(context).Add(
                              quote.AgentId,
                              (int)Constant.NotificationType.Accept,
                              quote.Id,
                              ConfigurationHelper.NOTIFICATION_TITLE,
                              Constant.Notification.ACCCEPTED_TEXT);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Save([FromBody] RequestQuotationViewModel model)
        {
            try
            {
                if (model == null)
                    return NotFound();
                var quotation = new RequestQuotation
                {
                    Id = model.Id,
                    ServiceRequestId = model.ServiceRequestId,
                    AgentId = model.AgentId,
                    QuotationTemplateId = model.QuotationTemplateId,
                    QuotationText = model.QuotationText,
                    Premimum = Convert.ToDecimal(model.Premimum.Replace(",", String.Empty)),
                    Status = (int)Constant.QuotationStatus.Initial,
                    Cover = Convert.ToDecimal(model.Cover.Replace(",", String.Empty))
                };

                using (AppDBContext context = new AppDBContext())
                {
                    long quoteId = new RequestQuotationRepository(context).Save(quotation);
                    new AgentServiceRequestRepository(context).UpdateResponseTime(model.ServiceRequestId, model.AgentId);
                    var userRepo = new UserRepository(context);
                    var request = new ServiceRequestRepository(context).GetById(model.ServiceRequestId);
                    var agentProfile = new UserProfileRepository(context).GetByUserId(model.AgentId);
                    var agent = userRepo.GetByUserId(model.AgentId);
                    var agentName = agent.Name;
                    var company = agent.Company.Name;
                    if (agentProfile != null)
                        agentName = agentProfile.FirstName + " " + agentProfile.LastName;

                    MessageModel message = new MessageModel
                    {
                        MessageText = "Quotation Sent by: " + agentName + "\n" + company,
                        RequestId = model.ServiceRequestId,
                        SenderId = model.AgentId,
                        RecieverId = request.UserId,
                        QuotationId = quoteId
                    };
                    AddMessage(message, context);
                    new NotificationRepository(context).Add(
                            message.RecieverId,
                            (int)Constant.NotificationType.Quotation,
                            message.QuotationId,
                            ConfigurationHelper.NOTIFICATION_TITLE,
                            Constant.Notification.NEW_QUOTATION_TEXT);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult AcceptByMessageThread(long ThreadId)
        {
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var thread = new MessageThreadRepository(context).GetById(ThreadId);
                    var repo = new RequestQuotationRepository(context);
                    var quote = repo.GetByRequestAndAgent(thread.RequestId, thread.AgentId);
                    quote.Status = (int)Constant.QuotationStatus.Accepted;
                    repo.Update(quote);
                    var otherQuotes = repo.GetByRequest(quote.ServiceRequestId)?.Where(q => q.Id != quote.Id);
                    if (otherQuotes != null)
                    {
                        foreach (var q in otherQuotes)
                        {
                            q.Status = (int)Constant.QuotationStatus.Closed;
                            repo.Update(q);
                        }
                    }

                    var reqRepo = new ServiceRequestRepository(context);
                    var request = quote.ServiceRequest;
                    request.Status = (int)Constant.ServiceRequestStatus.Closed;
                    reqRepo.Update(request);
                    var buyerName = new UserRepository(context).GetName(request.UserId);
                    MessageModel message = new MessageModel
                    {
                        MessageText = "Quotation accepted by: " + buyerName,
                        RequestId = request.Id,
                        SenderId = request.UserId,
                        RecieverId = quote.AgentId,
                        QuotationId = 0
                    };
                    AddMessage(message, context);

                    new NotificationRepository(context).Add(
                              quote.AgentId,
                              (int)Constant.NotificationType.Accept,
                              quote.Id,
                              ConfigurationHelper.NOTIFICATION_TITLE,
                              Constant.Notification.ACCCEPTED_TEXT);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        private void AddMessage(MessageModel Model, AppDBContext context)
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
                    QuotationId = Model.QuotationId,
                    Time = DateTime.Now.ToUniversalTime()
                };

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
                    }

                    var existingthread =
                        new MessageThreadRepository(context).GetByAgentId(agentId).Where(
                            t => t.RequestId == Model.RequestId && t.BuyerId == buyerId).FirstOrDefault();

                    if (existingthread == null)
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

                Id = new MessageRepository(context).Add(message);
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }            
        }
    }


}
