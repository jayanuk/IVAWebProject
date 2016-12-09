using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IClaimType
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
    }
}
