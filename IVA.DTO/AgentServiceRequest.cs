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
}
