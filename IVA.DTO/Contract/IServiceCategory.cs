using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceCategory
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
    }
}
