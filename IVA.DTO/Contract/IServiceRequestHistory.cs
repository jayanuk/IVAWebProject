using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceRequestHistory
    {
        long Id { get; set; }
        long ServiceRequestId { get; set; }
        long UserId { get; set; }
        string Oldvalue { get; set; }
        string NewValue { get; set; }
        string ChangeType { get; set; }
        string Description { get; set; }
        DateTime TimeOccured { get; set; }
    }
}
