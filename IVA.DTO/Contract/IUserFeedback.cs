using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IUserFeedback
    {
        long Id { get; set; }
        int Rating { get; set; }
        string Description { get; set; }
        DateTime Date { get; set; }
        long UserId { get; set; }
    }
}
