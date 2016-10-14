using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class ServiceRequest : IServiceRequest
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public long UserId { get; set; }
        public DateTime TimeOccured { get; set; }

        public int ClaimType { get; set; }
        public int UsageType { get; set; }
        public int RegistrationCategory { get; set; }

        public decimal Vehiclevalue { get; set; }
        public string VehicleNo { get; set; }
        public int Status { get; set; }
    }
}
