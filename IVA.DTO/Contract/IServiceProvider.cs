using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IServiceProvider
    {
        long Id { get; set; }
        string Name { get; set; }
        int ServiceCategoryId { get; set; }        
        string WebSite { get; set; }
        string Email { get; set; }
        bool IsActive { get; set; }
    }
}
