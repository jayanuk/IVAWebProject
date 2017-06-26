using IVA.Common;
using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class ServiceRequestRepository : BaseRepository
    {
        public ServiceRequestRepository(AppDBContext context) : base(context)
        {            
        }

        public ServiceRequest GetById(long Id)
        {
            return context.ServiceRequests.Where(r => r.Id == Id).FirstOrDefault();
        }

        public List<IServiceRequest> GetByBuyerId(long Id, int Status, int Page)
        {
            if(Status > 0)
                return context.ServiceRequests.Where(r => r.UserId == Id && r.Status == Status).
                    OrderByDescending(r => r.TimeOccured).
                    Skip((Page - 1) * Constant.Paging.BUYER_REQUESTS_PER_PAGE).
                       Take(Constant.Paging.BUYER_REQUESTS_PER_PAGE).ToList<IServiceRequest>();
            else
                return context.ServiceRequests.Where(r => r.UserId == Id).
                    OrderByDescending(r => r.TimeOccured).
                    Skip((Page - 1) * Constant.Paging.BUYER_REQUESTS_PER_PAGE).
                       Take(Constant.Paging.BUYER_REQUESTS_PER_PAGE).ToList<IServiceRequest>();
        }

        public List<IServiceRequest> GetByAgentId(long AgentId)
        {
            var agentRequests = new AgentServiceRequestRepository(context).GetByAgentId(AgentId);
            var serviceRequests = agentRequests.Select(a => a.ServiceRequest).ToList<IServiceRequest>();
            return serviceRequests;
        }

        public List<IServiceRequest> GetPendingByAgentId(long AgentId)
        {
            var agentRequests = new AgentServiceRequestRepository(context).GetPendingByAgentId(AgentId);
            var serviceRequests = agentRequests.Select(a => a.ServiceRequest).ToList<IServiceRequest>();
            return serviceRequests;
        }

        public int GetPendingByAgentIdCount(long AgentId)
        {
            var agentRequests = new AgentServiceRequestRepository(context).GetPendingByAgentId(AgentId);
            int count = agentRequests.Where(ar => ar.Status == (int)Constant.ServiceRequestStatus.Initial).Count();
            return count;
        }

        public List<IServiceRequest> GetFollowUpByAgentId(long AgentId)
        {
            var agentRequests = new AgentServiceRequestRepository(context).GetForFollowUpByAgentId(AgentId);
            var serviceRequests = agentRequests.Select(a => a.ServiceRequest).ToList<IServiceRequest>().
                    Where(r => r.TimeOccured.AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).AddHours(-ConfigurationHelper.HOURS_TO_FOLLOW_UP) <= DateTime.UtcNow).ToList();
          
            return serviceRequests;
        }

        public List<IServiceRequest> GetFollowUpByBuyerId(long BuyerId)
        {
            //var serviceRequests = context.ServiceRequests.Where(
            //    r => !(r.BuyerResponded ?? false) && r.UserId == BuyerId && 
            //    r.Status != (int)Constant.ServiceRequestStatus.Closed && 
            //    r.Status != (int)Constant.ServiceRequestStatus.Expired).ToList().Where( 
            //    r =>r.TimeOccured.AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST) > DateTime.UtcNow &&
            //    r.TimeOccured.AddDays(ConfigurationHelper.DAYS_TO_EXPIRE_REQUEST).AddHours(-ConfigurationHelper.HOURS_TO_FOLLOW_UP) <= DateTime.UtcNow).ToList();

            var serviceRequests = from sr in context.ServiceRequests.Where(
                                    r => r.UserId == BuyerId &&
                                    r.Status != (int)Constant.ServiceRequestStatus.Closed &&
                                    r.Status != (int)Constant.ServiceRequestStatus.Expired)
                                    join q in context.RequestQuotations on sr.Id equals q.ServiceRequestId
                                    where sr.UserId == BuyerId && (q.Status == (int)Constant.QuotationStatus.Initial)
                                  select sr;

            return serviceRequests.ToList<IServiceRequest>();
        }

        public void UpdateBuyerResponded(long RequestId)
        {
            var request = context.ServiceRequests.Where(r => r.Id == RequestId).FirstOrDefault();
            if(request != null)
            {
                request.BuyerResponded = true;
                context.SaveChanges();
            }
        }

        public long Add(ServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = new ServiceRequest();
            sr.InsuranceTypeId = ServiceRequestInstance.InsuranceTypeId;
            sr.Code = ServiceRequestInstance.Code;            
            sr.Status = (int)Constant.ServiceRequestStatus.Initial;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;            
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.VehicleValue = ServiceRequestInstance.VehicleValue;
            sr.VehicleYear = ServiceRequestInstance.VehicleYear;
            sr.TimeOccured = DateTime.Now.ToUniversalTime();
            sr.IsFinanced = ServiceRequestInstance.IsFinanced;
            sr.Location = ServiceRequestInstance.Location;
            sr.ClientType = ServiceRequestInstance.ClientType;

            context.ServiceRequests.Add(sr);
            context.SaveChanges();
            UpdateSRCode(sr.Id);
            return sr.Id;
        }

        public void Update(ServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestInstance.Id).FirstOrDefault();
            if (sr == null)
                return;

            sr.InsuranceTypeId = ServiceRequestInstance.InsuranceTypeId;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.VehicleValue = ServiceRequestInstance.VehicleValue;
            sr.VehicleYear = ServiceRequestInstance.VehicleYear;
            sr.IsFinanced = ServiceRequestInstance.IsFinanced;
            sr.Location = ServiceRequestInstance.Location;
            context.SaveChanges();
        }

        public void UpdateSRStatus(long ServiceRequestId, int Status)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestId).FirstOrDefault();
            if (sr == null)
                return;
            sr.Status = Status;
            context.SaveChanges();
        }

        private void UpdateSRCode(long ServiceRequestId)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestId).FirstOrDefault();
            sr.Code = "SR-" + String.Format("{0:D6}", sr.Id);
            context.SaveChanges();
            
        }

        public void AddQuotation(RequestQuotation Quotation)
        {
            var existing = context.RequestQuotations.Where(q => q.ServiceRequestId == Quotation.ServiceRequestId).FirstOrDefault();
            if(existing == null)
                context.RequestQuotations.Add(Quotation);
        }

        public int GetPendingRequestCount(long AgentId)
        {
            return GetPendingByAgentIdCount(AgentId);
        }
    }
}
