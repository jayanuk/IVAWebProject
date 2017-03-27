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
        long QuotationTemplateId { get; set; }
        decimal? Premimum { get; set; }
        decimal? Cover { get; set; }
        long AgentId { get; set; }
        int? Status { get; set; }
        string QuotationText { get; set; }
        DateTime? CreatedTime { get; set; }
        DateTime? ModifiedTime { get; set; }
        bool? IsExpired { get; set; }
    }
}
