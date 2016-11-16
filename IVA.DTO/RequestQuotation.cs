using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class RequestQuotation : IRequestQuotation
    {
        [Key]
        public long Id { get; set; }
        public long ServiceRequestId { get; set; }
        public int QuotationId { get; set; }
        public long SellerId { get; set; }
        public DateTime Time { get; set; }
    }
}
