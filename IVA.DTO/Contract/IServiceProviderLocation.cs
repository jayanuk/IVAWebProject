using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceProviderLocation
    {
        long Id { get; set; }
        long ServiceProviderId { get; set; }
        string Name { get; set; }
        bool IsActive { get; set; }
        string Province { get; set; }
        string District { get; set; }
        string City { get; set; }
        string Street1 { get; set; }
        string Street2 { get; set; }
        string Mobile { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }              
        double Longitude { get; set; }
        double Latitude { get; set; }
    }
}
