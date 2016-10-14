using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class QuotationTemplate : IQuotationTemplate
    {
        public long Id { get; set; }
        public int ValidityId { get; set; }
        public string Body { get; set; }
        public decimal Amount { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ModifiedBy { get; set; }
    }
}
