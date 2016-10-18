using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class ServiceRequestHistory : IServiceRequestHistory
    {
        public long Id { get; set; }
        public long ServiceRequestId { get; set; }
        public long UserId { get; set; }
        public string Oldvalue { get; set; }
        public string NewValue { get; set; }
        public string ChangeType { get; set; }
        public string Description { get; set; }
        public DateTime TimeOccured { get; set; }
    }
}
