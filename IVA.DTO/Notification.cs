using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }
        public long RecieverId { get; set; }
        public int Type { get; set; }
        public long? RecordId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        public DateTime Time { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsVisited { get; set; }
        [NotMapped]
        public string DisplayTime { get; set; }
    }
}
