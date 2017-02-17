using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class AgentServiceRequest : IAgentServiceRequest
    {
        public long Id { get; set; }
        public long ServiceRequestId { get; set; }
        public long AgentId { get; set; }
        public int? Status { get; set; }
        public DateTime? ResponseTime { get; set; }
        public DateTime? CreatedTime { get; set; }

        public virtual ServiceRequest ServiceRequest { get; set; }
    }
        
    public class AgentServiceRequestComparer : IEqualityComparer<AgentServiceRequest>
    {        
        public bool Equals(AgentServiceRequest x, AgentServiceRequest y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.ServiceRequestId == y.ServiceRequestId;
        }

        public int GetHashCode(AgentServiceRequest request)
        {
            if (Object.ReferenceEquals(request, null)) return 0;
            return request.ServiceRequestId.GetHashCode();
        }

    }

}
