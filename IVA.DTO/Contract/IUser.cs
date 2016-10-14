using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IUser
    {
        long Id { get; set; }
        long? LoginId { get; set; }
        int UserType { get; set; }   
        string Name { get; set; }
        string UserName { get; set; }        
        string Password { get; set; }
        bool PasswordValidated { get; set; }
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
