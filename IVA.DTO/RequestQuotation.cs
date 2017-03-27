using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public long QuotationTemplateId { get; set; }
        public decimal? Premimum { get; set; }
        public decimal? Cover { get; set; }
        public long AgentId { get; set; }
        public int? Status { get; set; }
        public string QuotationText { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool? IsExpired { get; set; }

        public virtual ServiceRequest ServiceRequest { get; set; }
        public virtual QuotationTemplate QuotationTemplate { get; set; }        
        public virtual User Agent { get; set; }
    }
}
