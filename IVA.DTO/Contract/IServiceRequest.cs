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
        string Description { get; set; }

        long UserId { get; set; }
        DateTime TimeOccured { get; set; }

        int ClaimType { get; set; }
        int UsageType { get; set; }
        int RegistrationCategory { get; set; }

        decimal Vehiclevalue { get; set; }
        string VehicleNo { get; set; }
        int Status { get; set; }
    }
}
