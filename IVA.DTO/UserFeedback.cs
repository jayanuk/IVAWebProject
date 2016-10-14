using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class UserFeedback : IUserFeedback
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
    }
}
