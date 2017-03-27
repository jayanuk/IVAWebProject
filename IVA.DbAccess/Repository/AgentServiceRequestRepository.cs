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
    public class AgentServiceRequestRepository : BaseRepository
    {
        public AgentServiceRequestRepository(AppDBContext context) : base(context)
        {

        }

        public long Add(AgentServiceRequest AgentRequest)
        {
            context.AgentServiceRequests.Add(AgentRequest);
            context.SaveChanges();
            return AgentRequest.Id;
        }

        public List<AgentServiceRequest> GetByAgentId(long AgentId)
        {
            var result = context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId &&
                (i.Status == (int)Constant.ServiceRequestStatus.SellerResponded || i.Status == (int)Constant.ServiceRequestStatus.Closed) &&                
                i.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Expired).ToList();
            return result.Distinct<AgentServiceRequest>(new AgentServiceRequestComparer()).ToList();
        }

        public List<AgentServiceRequest> GetByAgentIdForAssign(long AgentId)
        {
            List<int> quoteOpenStatus = new List<int> { (int)Constant.QuotationStatus.Initial, (int)Constant.QuotationStatus.Checked };
            var result = context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId &&                
                (i.Status == (int)Constant.ServiceRequestStatus.Initial || i.Status == (int)Constant.ServiceRequestStatus.PendingResponse ||i.Status == (int)Constant.ServiceRequestStatus.SellerResponded) &&
                i.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Closed &&
                i.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Expired && 
                !i.ServiceRequest.QuotationList.Any(q => q.AgentId == AgentId && quoteOpenStatus.Any(s => s == q.Status) && !(q.IsExpired ?? false))).ToList();
            return result.Distinct<AgentServiceRequest>(new AgentServiceRequestComparer()).ToList();
        }

        public List<IAgentServiceRequest> GetPendingByAgentId(long AgentId)
        {
            return context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId &&
                (i.Status == (int)Constant.ServiceRequestStatus.PendingResponse || i.Status == (int)Constant.ServiceRequestStatus.Initial) &&
                 (i.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Closed &&
                i.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Expired)).ToList<IAgentServiceRequest>();
        }

        public List<IAgentServiceRequest> GetForFollowUpByAgentId(long AgentId)
        {
            //var result = from asr in context.AgentServiceRequests.Where(
            //    s => s.AgentId == AgentId && s.Status != (int)Constant.ServiceRequestStatus.SellerResponded)
            //             join t in context.MessageThreads on asr.ServiceRequestId equals t.RequestId
            //             where !t.Messages.Any(m => m.SenderId == asr.ServiceRequest.UserId)
            //             select asr;
            var result = from asr in context.AgentServiceRequests.Where(s => s.AgentId == AgentId &&
                         s.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Closed && s.ServiceRequest.Status != (int)Constant.ServiceRequestStatus.Expired)
                         join q in context.RequestQuotations on asr.ServiceRequestId equals q.ServiceRequestId
                         where q.AgentId == asr.AgentId && (q.Status == (int)Constant.QuotationStatus.Initial || q.Status == (int)Constant.QuotationStatus.Checked)
                         select asr;
            return result.ToList<IAgentServiceRequest>();
        }

        public void UpdateToPending(long RequestId, long AgentId)
        {
            var existing = context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId && i.ServiceRequestId == RequestId).FirstOrDefault();
            if (existing != null && existing.Status == (int)Constant.ServiceRequestStatus.Initial)
            {
                existing.Status = (int)Constant.ServiceRequestStatus.PendingResponse;
                context.SaveChanges();
            }
        }

        public void UpdateResponseTime(long RequestId, long AgentId)
        {
            var existing = context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId && i.ServiceRequestId == RequestId).FirstOrDefault();
            if(existing != null)
            {
                existing.Status = (int)Constant.ServiceRequestStatus.SellerResponded;
                existing.ResponseTime = DateTime.Now.ToUniversalTime();
                context.SaveChanges();
            }
        }

        public long GetAgentIdIfServicedRecently(string VehicleNo, int CompanyId)
        {
            if (!String.IsNullOrEmpty(VehicleNo))
                VehicleNo = VehicleNo.Trim();
            var cutoffDate = DateTime.UtcNow.AddMonths(-2);
            //Check to see if there is one who took action for vehicle no
            long? result = (from sr in context.ServiceRequests.Where(r => r.VehicleNo == VehicleNo)
                         join asr in context.AgentServiceRequests on sr.Id equals asr.ServiceRequestId
                         join user in context.Users on asr.AgentId equals user.Id
                         join quote in context.RequestQuotations on asr.AgentId equals quote.AgentId
                         where quote.ServiceRequestId == sr.Id && user.CompanyId == CompanyId && sr.TimeOccured > cutoffDate
                         orderby asr.CreatedTime ascending
                         select asr.AgentId).FirstOrDefault();
            //If there is none who took action then get the first one
            if( result == null || result == 0)
                result = (from sr in context.ServiceRequests.Where(r => r.VehicleNo == VehicleNo)
                          join asr in context.AgentServiceRequests on sr.Id equals asr.ServiceRequestId
                          join user in context.Users on asr.AgentId equals user.Id                          
                          where user.CompanyId == CompanyId && sr.TimeOccured > cutoffDate
                          orderby asr.CreatedTime ascending
                          select asr.AgentId).FirstOrDefault();
            return result ?? 0;

        }

    }
}
