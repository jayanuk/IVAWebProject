using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class ServiceProviderLocation : IServiceProviderLocation
    {
        public long Id { get; set; }
        public long ServiceProviderId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
