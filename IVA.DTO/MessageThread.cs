﻿using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class MessageThread : IMessageThread
    {
        [Key]
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long BuyerId { get; set; }
        public long AgentId { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}
