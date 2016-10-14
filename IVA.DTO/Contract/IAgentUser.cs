using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IAgentUser
    {
        long Id { get; set; }
        long UserId { get; set; }
        long CompanyId { get; set; }
        bool IsActive { get; set; }        
    }
}
