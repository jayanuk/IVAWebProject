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

        public List<IAgentServiceRequest> GetByAgentId(long AgentId)
        {
            return context.AgentServiceRequests.Where(i => i.AgentId == AgentId).ToList<IAgentServiceRequest>();
        }

        public void UpdateResponseTime(long RequestId, long AgentId)
        {
            var existing = context.AgentServiceRequests.Where(
                i => i.AgentId == AgentId && i.ServiceRequestId == RequestId).FirstOrDefault();
            if(existing != null)
            {
                existing.Status = (int)Constant.ServiceRequestStatus.SellerResponded;
                existing.ResponseTime = DateTime.Now;
                context.SaveChanges();
            }
        }

    }
}
