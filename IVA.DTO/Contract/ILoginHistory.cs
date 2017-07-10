using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface ILoginHistory
    {
        long Id { get; set; }
        long UserId { get; set; }
        int? ClientType { get; set; }
        DateTime Time { get; set; }
    }
}
