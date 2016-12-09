using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IMessageThread
    {
        long Id { get; set; }       
        long RequestId { get; set; }
        long BuyerId { get; set; }
        long AgentId { get; set; }
        DateTime CreatedTime { get; set; }
    }
}
