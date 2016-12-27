using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class Message : IMessage
    {
        [Key]
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public long SenderId { get; set; }
        public long RecieverId { get; set; }
        public string MessageText { get; set; }
        public int Status { get; set; }
        public DateTime Time { get; set; }    
        public long? QuotationId { get; set; }   
    }
}
