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
        long ThreadId { get; set; }        
        long SenderId { get; set; }
        long RecieverId { get; set; }
        string MessageText { get; set; }        
        int Status { get; set; }        
        DateTime Time { get; set; }        
    }
}
