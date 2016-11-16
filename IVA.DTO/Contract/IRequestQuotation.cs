using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IRequestQuotation
    {
        long Id { get; set; }
        long ServiceRequestId { get; set; }
        int QuotationId { get; set; }
        long SellerId { get; set; }
        DateTime Time { get; set; }
    }
}
