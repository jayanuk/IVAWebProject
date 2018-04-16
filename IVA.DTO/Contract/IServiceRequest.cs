using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceRequest
    {
        long Id { get; set; }
        string Code { get; set; }
        int InsuranceTypeId { get; set; }
        long UserId { get; set; }
        DateTime TimeOccured { get; set; }

        int ClaimType { get; set; }
        int UsageType { get; set; }
        int RegistrationCategory { get; set; }
        decimal VehicleValue { get; set; }
        string VehicleNo { get; set; }
        int VehicleYear { get; set; }
        bool IsFinanced { get; set; }
        int Status { get; set; }
        bool? BuyerResponded { get; set; }
        string Location { get; set; }
        int? ClientType { get; set; }
        int? FuelType { get; set; }

        List<VehicleImage> Images { get; set; }
        List<RequestQuotation> QuotationList { get; set; }
    }
}
