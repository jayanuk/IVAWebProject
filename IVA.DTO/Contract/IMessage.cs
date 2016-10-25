using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IMessage
    {
        long Id { get; set; }
        long SRId { get; set; }
        string MessageText { get; set; }
        int SequenceId { get; set; }
        int Status { get; set; }
        long UserId { get; set; }
        DateTime CreatedTime { get; set; }
        int Type { get; set; } //Request from buyer or response from seller
        long RequestId { get; set; } //Parent message id where this message is in reply to
    }
}
