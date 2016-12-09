using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IAgentServiceRequest
    {
        long Id { get; set; }
        long ServiceRequestId { get; set; }
        long AgentId { get; set; }
        int? Status { get; set; }
        DateTime? ResponseTime { get; set; }
        DateTime? CreatedTime { get; set; }

        ServiceRequest ServiceRequest { get; set; }
    }
}
