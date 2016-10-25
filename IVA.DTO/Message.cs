using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class Message : IMessage
    {
        [Key]
        public long Id { get; set; }
        public long SRId { get; set; }
        public string MessageText { get; set; }
        public int SequenceId { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Type { get; set; } //Request from buyer or response from seller
        public long RequestId { get; set; } //Parent message id whe
    }
}
