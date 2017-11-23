using Core.Log;
using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.DTO.Contract;
using IVA.FindExpert.Helpers;
using IVA.FindExpert.Helpers.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class ServiceRequestController : BaseController
    {
        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetByBuyerId(long BuyerId,int Status, int Page)
        {
            var requests = new List<ServiceRequestModel>();
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var serviceReqRepo = new ServiceRequestRepository(context);
                    if (!(Status == (int)Constant.ServiceRequestStatus.Closed || Status == (int)Constant.ServiceRequestStatus.Expired))
                        Status = 0;
                    var result = serviceReqRepo.GetByBuyerId(BuyerId, Status, Page).ToList();                  

                    requests = result.Select(i => new ServiceRequestModel
                        {
                            Id = i.Id,
                            Code = i.Code,
                            InsuranceTypeId = i.InsuranceTypeId,
                            UserId = i.UserId,
                            CreatedDate = i.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                            ClaimType = i.ClaimType,
                            UsageType = i.UsageType,
                            RegistrationCategory = i.RegistrationCategory,
                            VehicleNo = i.VehicleNo,
                            VehicleValue = i.VehicleValue,
                            VehicleYear = i.VehicleYear,
                            IsFinanced = i.IsFinanced,
                            Status = i.Status,
                            ExpiryDate = i.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                            QuotationList = GetServiceQuotations(i.Id)
                        }).ToList();

                    var followUp = serviceReqRepo.GetFollowUpByBuyerId(BuyerId);
                    foreach (var sr in followUp)
                    {
                        var req = requests.Where(r => r.Id == sr.Id).FirstOrDefault();
                        if (req != null)
                        {
                            req.IsFollowUp = true;
                        }
                    }
                }
                return Json(requests);
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetByAgentId(long AgentId, int Page)
        {
            var requests = new List<ServiceRequestModel>();
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    requests = new ServiceRequestRepository(context).GetByAgentId(AgentId).
                        OrderByDescending(r => r.TimeOccured).
                        Select(
                        i => new ServiceRequestModel
                        {
                            Id = i.Id,
                            Code = i.Code,
                            InsuranceTypeId = i.InsuranceTypeId,
                            UserId = i.UserId,
                            CreatedDate = i.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                            ClaimType = i.ClaimType,
                            UsageType = i.UsageType,
                            RegistrationCategory = i.RegistrationCategory,
                            VehicleNo = i.VehicleNo,
                            VehicleValue = i.VehicleValue,
                            VehicleYear = i.VehicleYear,
                            IsFinanced = i.IsFinanced,
                            Status = i.Status,
                            Location = i.Location == null ? String.Empty : i.Location,
                            ExpiryDate = i.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                            QuotationList = GetServiceQuotations(i.Id)
                        }).ToList();
                }
                requests = requests.Skip((Page - 1) * Constant.Paging.AGENT_REQUESTS_PER_PAGE).
                       Take(Constant.Paging.AGENT_REQUESTS_PER_PAGE).ToList();
                return Json(requests);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetPendingByAgentId(long AgentId)
        {
            var requests = new List<ServiceRequestModel>();
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    requests = new ServiceRequestRepository(context).GetPendingByAgentId(AgentId).
                        Where(r => r.Status != (int)Constant.ServiceRequestStatus.Expired ||
                        r.Status != (int)Constant.ServiceRequestStatus.Closed).
                        OrderByDescending(r => r.TimeOccured).
                        Select(
                        i => new ServiceRequestModel
                        {
                            Id = i.Id,
                            Code = i.Code,
                            InsuranceTypeId = i.InsuranceTypeId,
                            UserId = i.UserId,
                            CreatedDate = i.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                            ClaimType = i.ClaimType,
                            UsageType = i.UsageType,
                            RegistrationCategory = i.RegistrationCategory,
                            VehicleNo = i.VehicleNo,
                            VehicleValue = i.VehicleValue,
                            VehicleYear = i.VehicleYear,
                            IsFinanced = i.IsFinanced,
                            Status = i.Status,
                            Location = i.Location == null ? String.Empty : i.Location,
                            ExpiryDate = i.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                            QuotationList = GetServiceQuotations(i.Id)
                        }).ToList();
                }
                return Json(requests);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetFollowUpByAgentId(long AgentId)
        {
            var requests = new List<ServiceRequestModel>();
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    requests = new ServiceRequestRepository(context).GetFollowUpByAgentId(AgentId).
                        OrderByDescending(r => r.TimeOccured).
                        Select(
                        i => new ServiceRequestModel
                        {
                            Id = i.Id,
                            Code = i.Code,
                            InsuranceTypeId = i.InsuranceTypeId,
                            UserId = i.UserId,
                            CreatedDate = i.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                            ClaimType = i.ClaimType,
                            UsageType = i.UsageType,
                            RegistrationCategory = i.RegistrationCategory,
                            VehicleNo = i.VehicleNo,
                            VehicleValue = i.VehicleValue,
                            VehicleYear = i.VehicleYear,
                            IsFinanced = i.IsFinanced,
                            Status = i.Status,
                            Location = i.Location == null ? String.Empty : i.Location,
                            ExpiryDate = i.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                            TimeToExpire = getTimeToExpire(i.TimeOccured),
                            QuotationList = GetServiceQuotations(i.Id)
                        }).ToList();
                }
                return Json(requests);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetFollowUpByAgentCount(long AgentId)
        {
            var count = 0;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    count = new ServiceRequestRepository(context).GetFollowUpByAgentId(AgentId).Count();
                }
                return Json(count);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetFollowUpByBuyerCount(long BuyerId)
        {
            var count = 0;
            using (AppDBContext context = new AppDBContext())
            {
                count = new ServiceRequestRepository(context).GetFollowUpByBuyerId(BuyerId).Count();
            }
            return Json(count);
        }

        private string getTimeToExpire(DateTime CreatedDate)
        {
            var expDate = CreatedDate.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST);
            var timeToexpire = expDate.Subtract(DateTime.Now.ToUniversalTime().GetAdjustedTime());
            if (timeToexpire.Hours > 0)
                return timeToexpire.Hours.ToString() + "h " + timeToexpire.Minutes + "m";
            else
                return timeToexpire.Minutes + "mins";
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetById(long Id, long? UserId)
        {
            ServiceRequest request = null;
            ServiceRequestModel model = null;
            IUser buyer = null;
            IUserProfile buyerProfile = null;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    request = new ServiceRequestRepository(context).GetById(Id);
                    buyer = new UserRepository(context).GetByUserId(request.UserId);
                    buyerProfile = new UserProfileRepository(context).GetByUserId(request.UserId);
                    if (UserId != null)
                        new AgentServiceRequestRepository(context).UpdateToPending(
                            Id, UserId ?? 0);
                }

                if (request != null)
                {
                    model = new ServiceRequestModel
                    {
                        Id = request.Id,
                        Code = request.Code,
                        InsuranceTypeId = request.InsuranceTypeId,
                        UserId = request.UserId,
                        CreatedDate = request.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                        ClaimType = request.ClaimType,
                        UsageType = request.UsageType,
                        RegistrationCategory = request.RegistrationCategory,
                        VehicleNo = request.VehicleNo,
                        VehicleValue = request.VehicleValue,
                        VehicleYear = request.VehicleYear,
                        IsFinanced = request.IsFinanced,
                        Status = request.Status,
                        Location = request.Location == null ? String.Empty : request.Location,
                        ExpiryDate = request.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                        QuotationList = GetServiceQuotations(request.Id)                        
                    };

                    if (buyer != null)
                    {
                        model.BuyerName = buyer.Name;
                        model.BuyerMobile = buyer.UserName;
                        model.IsAllowPhone = true;

                        if (buyerProfile != null)
                        {
                            model.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;
                            model.BuyerPhone = buyerProfile.Phone;
                            if (String.IsNullOrEmpty(model.Location))
                                model.Location = buyerProfile.City;
                            if (buyerProfile.ContactMethod == (int)Constant.ContactMethod.Message)
                                model.IsAllowPhone = false;
                        }
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetById(long Id)
        {
            ServiceRequest request = null;
            ServiceRequestModel model = null;
            IUser buyer = null;
            IUserProfile buyerProfile = null;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    request = new ServiceRequestRepository(context).GetById(Id);
                    buyer = new UserRepository(context).GetByUserId(request.UserId);
                    buyerProfile = new UserProfileRepository(context).GetByUserId(request.UserId);
                }

                if (request != null)
                {
                    model = new ServiceRequestModel
                    {
                        Id = request.Id,
                        Code = request.Code,
                        InsuranceTypeId = request.InsuranceTypeId,
                        UserId = request.UserId,
                        CreatedDate = request.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                        ClaimType = request.ClaimType,
                        UsageType = request.UsageType,
                        RegistrationCategory = request.RegistrationCategory,
                        VehicleNo = request.VehicleNo,
                        VehicleValue = request.VehicleValue,
                        VehicleYear = request.VehicleYear,
                        IsFinanced = request.IsFinanced,
                        Status = request.Status,
                        Location = request.Location == null ? String.Empty : request.Location,
                        ExpiryDate = request.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                        QuotationList = GetServiceQuotations(request.Id)
                    };

                    if (buyer != null)
                    {
                        model.BuyerName = buyer.Name;
                        model.BuyerMobile = buyer.UserName;
                        model.IsAllowPhone = false;

                        if (buyerProfile != null)
                        {
                            model.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;
                            model.BuyerPhone = buyerProfile.Phone;
                            if (String.IsNullOrEmpty(model.Location))
                                model.Location = buyerProfile.City;
                            if (buyerProfile.ContactMethod != (int)Constant.ContactMethod.Message)
                                model.IsAllowPhone = true;
                        }
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult GetByQuoteId(long QuotationId, long? UserId)
        {
            ServiceRequest request = null;
            ServiceRequestModel model = null;
            IUser buyer = null;
            IUserProfile buyerProfile = null;
            RequestQuotation quote = null;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    quote = new RequestQuotationRepository(context).GetById(QuotationId);
                    request = quote.ServiceRequest;
                    buyer = new UserRepository(context).GetByUserId(request.UserId);
                    buyerProfile = new UserProfileRepository(context).GetByUserId(request.UserId);
                    if (UserId != null)
                        new AgentServiceRequestRepository(context).UpdateToPending(
                            request.Id, UserId ?? 0);
                }

                if (request != null)
                {
                    model = new ServiceRequestModel
                    {
                        Id = request.Id,
                        Code = request.Code,
                        InsuranceTypeId = request.InsuranceTypeId,
                        UserId = request.UserId,
                        CreatedDate = request.TimeOccured.GetAdjustedTime().ToString("yyyy-MM-dd"),
                        ClaimType = request.ClaimType,
                        UsageType = request.UsageType,
                        RegistrationCategory = request.RegistrationCategory,
                        VehicleNo = request.VehicleNo,
                        VehicleValue = request.VehicleValue,
                        VehicleYear = request.VehicleYear,
                        IsFinanced = request.IsFinanced,
                        Status = request.Status,
                        Location = request.Location == null ? String.Empty : request.Location,
                        ExpiryDate = request.TimeOccured.GetAdjustedTime().AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).ToString("yyyy-MM-dd"),
                        QuotationList = GetServiceQuotations(request.Id)
                    };

                    if (buyer != null)
                    {
                        model.BuyerName = buyer.Name;
                        model.BuyerMobile = buyer.UserName;
                        model.IsAllowPhone = true;

                        if (buyerProfile != null)
                        {
                            model.BuyerName = buyerProfile.FirstName + " " + buyerProfile.LastName;
                            model.BuyerPhone = buyerProfile.Phone;
                            if (String.IsNullOrEmpty(model.Location))
                                model.Location = buyerProfile.City;
                            if (buyerProfile.ContactMethod == (int)Constant.ContactMethod.Message)
                                model.IsAllowPhone = false;
                        }
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult Save(ServiceRequestModel Model)
        {
            ServiceRequest SR = new ServiceRequest();
            SR.Id = Model.Id;
            SR.InsuranceTypeId = Model.InsuranceTypeId;
            SR.Code = Model.Code;            
            SR.UserId = Model.UserId;
            SR.TimeOccured = DateTime.Now.ToUniversalTime();
            SR.ClaimType = Model.ClaimType;
            SR.UsageType = Model.UsageType;
            SR.RegistrationCategory = Model.RegistrationCategory;
            SR.VehicleNo = Model.VehicleNo;
            SR.VehicleValue = Model.VehicleValue;
            SR.VehicleYear = Model.VehicleYear;
            SR.IsFinanced = Model.IsFinanced;
            SR.Location = Model.Location;
            SR.ClientType = Model.ClientType;
            //SR.Images = Model.Images;

            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new ServiceRequestRepository(context);
                    if (SR.Id == 0)
                    {
                        SR.Id = repo.Add(SR);
                        AssignRequestToAgents(SR.Id);
                    }
                    else
                        repo.Update(SR);

                    if (SR.Images != null)
                    {
                        var imageRepo = new VehicleImageRepository(context);
                        imageRepo.Update(SR.Id, SR.Images);
                    }
                }

                return Json(SR);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [AccessActionFilter]
        public IHttpActionResult PendingRequestCount(long AgentId)
        {
            long count = 0;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    count = new ServiceRequestRepository(context).GetPendingRequestCount(AgentId);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }

            return Ok(count);
        }

        private List<RequestQuotationViewModel> GetServiceQuotations(long SRId)
        {
            List<RequestQuotationViewModel> models = null;
            List<RequestQuotation> quotes = null;
            using (AppDBContext context = new AppDBContext())
            {
                quotes = new RequestQuotationRepository(context).GetByRequest(SRId);
                var index = quotes.FindIndex(q => (q.Status ?? 0) == (int)Common.Constant.QuotationStatus.Accepted);
                if(index > 0)
                {
                    quotes = quotes.Where(q => (q.Status ?? 0) == (int)Common.Constant.QuotationStatus.Accepted).Concat(
                        quotes.Where(q => (q.Status ?? 0) != (int)Common.Constant.QuotationStatus.Accepted)).ToList();
                }

                if (quotes != null)
                {                    
                    models = quotes.Select(q => new RequestQuotationViewModel
                    {
                        Id = q.Id,
                        ServiceRequestId = q.ServiceRequestId,
                        QuotationTemplateId = q.QuotationTemplateId,
                        Premimum = q.Premimum?.ToString("#,##0.00"),
                        Cover = q.Cover?.ToString("#,##0.00"),
                        AgentId = q.AgentId,
                        AgentName = q.Agent?.Name,
                        AgentContact = q.Agent?.UserName,
                        QuotationTemplateName = q.QuotationTemplate.Name,
                        Status = q.Status ?? 0,
                        CompanyId = q.Agent.CompanyId ?? 0,
                        CompanyName = q.Agent.Company?.Name,
                        QuotationText = q.QuotationText
                        
                    }).ToList();
                }
            }
            return models;
        }
        
        private void AssignRequestToAgents(long ServiceRequestId)
        {
            try
            {
                //get all companies
                using (AppDBContext context = new AppDBContext())
                {
                    var companies = new CompanyRepository(context).GetAll();
                    var request = new ServiceRequestRepository(context).GetById(ServiceRequestId);
                    var agentServiceRepo = new AgentServiceRequestRepository(context);

                    foreach (var company in companies)
                    {
                        //Check vehicle number is recently serviced
                        var agentId = agentServiceRepo.GetAgentIdIfServicedRecently(request.VehicleNo, company.Id);
                        var lastRequestAgent = agentServiceRepo.GetLastOpenServiceAgentIdByCompany(company.Id);

                        if (agentId == 0)
                        {
                            //get agents for each company
                            var agents = new UserRepository(context).GetAgentsByCompany(company.Id);

                            if (agents == null || agents.Count == 0)
                                continue;

                            Dictionary<long, int> counts = new Dictionary<long, int>();

                            foreach (var agent in agents)
                            {
                                //get open requests for each agent
                                var agentRequests = agentServiceRepo.GetByAgentIdForAssign(agent.Id)?.Where(
                                    r => (r.Status ?? 0) != (int)Constant.ServiceRequestStatus.Closed ||
                                        (r.Status ?? 0) != (int)Constant.ServiceRequestStatus.Expired);
                                var weight = 0;
                                if (agent.Id == lastRequestAgent)
                                    weight = 100;

                                if (agentRequests != null)
                                    counts.Add(agent.Id, agentRequests.Count() + weight);
                                else
                                    counts.Add(agent.Id, 0 + weight);
                            }

                            //find agent with min no of requests
                            //order by id asc and get top 1 agent
                            var item = counts.OrderBy(i => i.Value).ThenBy(i => i.Key).First();
                            agentId = item.Key;
                        }                        

                        //assign the request to agent
                        AgentServiceRequest asr = new AgentServiceRequest
                        {
                            AgentId = agentId,
                            ServiceRequestId = ServiceRequestId,
                            Status = (int)Constant.ServiceRequestStatus.Initial,
                            CreatedTime = DateTime.Now.ToUniversalTime()
                        };

                        agentServiceRepo.Add(asr);

                        new NotificationRepository(context).Add(
                            asr.AgentId,
                            (int)Constant.NotificationType.Request,
                            asr.ServiceRequestId,
                            ConfigurationHelper.NOTIFICATION_TITLE,
                            Constant.Notification.NEW_REQUEST_TEXT);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(ServiceRequestController), ex.Message + ex.StackTrace, LogType.ERROR);
            }
        }
    }

}
