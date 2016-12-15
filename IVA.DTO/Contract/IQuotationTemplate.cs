using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IQuotationTemplate
    {
        long Id { get; set; }
        int ValidityId { get; set; }
        int CompanyId { get; set; }
        string Name { get; set; }
        string Body { get; set; }
        decimal? Amount { get; set; }
        DateTime? CreatedDate { get; set; }
        long? CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        long? ModifiedBy { get; set; }
    }
}
