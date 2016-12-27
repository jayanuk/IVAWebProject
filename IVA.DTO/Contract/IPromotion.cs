using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IPromotion
    {
        long Id { get; set; }
        int? InsuranceType { get; set; }
        int? Type { get; set; }
        string Title { get; set; }
        string Header { get; set; }
        string Description { get; set; }
        DateTime? CreatedDate { get; set; } 
        int? Status { get; set; }
    }
}
