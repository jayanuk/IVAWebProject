using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceRequestAgent
    {
        long Id { get; set; }
        long ServiceRequestID { get; set; }
        long BuyerId { get; set; }
        long AgentId { get; set; }
        int Status { get; set; }
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
