using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class Agent : IAgentUser
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public bool IsActive { get; set; }
    }
}
