using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IUserPasscode
    {
        long Id { get; set; }
        string Code { get; set; }
        string Phone { get; set; }
        string Name { get; set; }
        DateTime Time { get; set; }
    }
}
