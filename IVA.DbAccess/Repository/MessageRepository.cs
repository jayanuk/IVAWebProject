using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class MessageRepository : BaseRepository
    {
        public MessageRepository(AppDBContext context) : base(context)
        {

        }

        public long Add(Message Message)
        {            
            context.Messages.Add(Message);
            context.SaveChanges();
            return Message.Id;
        }

        public IMessage GetById(long MessageId)
        {
            return context.Messages.Where(m => m.Id == MessageId).FirstOrDefault();
        }

        public List<IMessage> GetByThreadId(long ThreadId)
        {
            return context.Messages.Where(m => m.ThreadId == ThreadId).ToList<IMessage>();
        }
        
    }
}
