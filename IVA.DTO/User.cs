using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class User : IUser
    {
        public long Id { get; set; }
        public long? LoginId { get; set; }
        public int UserType { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool PasswordValidated { get; set; }        
        public bool? Connected { get; set; }
        public string ConnectionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }        
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        
        public virtual Company Company { get; set; }
    }
}
