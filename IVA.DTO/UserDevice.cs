using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class UserDevice
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
