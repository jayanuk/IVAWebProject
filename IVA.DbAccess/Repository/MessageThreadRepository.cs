using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class MessageThreadRepository : BaseRepository
    {
        public MessageThreadRepository(AppDBContext context) : base(context)
        {
        }

        public long SafeAdd(MessageThread MessageThread)
        {
            long id = 0;
            var existing = GetByRequestId(MessageThread.RequestId);
            if (existing == null)
                id = Add(MessageThread);
            else
                id = existing.Id;

            return id;
        }

        public long Add(MessageThread MessageThread)
        {
            context.MessageThreads.Add(MessageThread);
            context.SaveChanges();
            return MessageThread.Id;
        }

        public IMessageThread GetById(long ThreadId)
        {
            return context.MessageThreads.Where(m => m.Id == ThreadId).FirstOrDefault();
        }

        public IMessageThread GetByRequestId(long RequestId)
        {
            return context.MessageThreads.Where(m => m.RequestId == RequestId).FirstOrDefault();
        }

        public List<IMessageThread> GetByBuyerId(long BuyerId)
        {
            return context.MessageThreads.Where(m => m.BuyerId == BuyerId).
                OrderByDescending(t => t.CreatedTime).ToList<IMessageThread>();
        }

        public List<IMessageThread> GetByAgentId(long AgentId)
        {
            return context.MessageThreads.Where(m => m.AgentId == AgentId).
                OrderByDescending(t => t.CreatedTime).ToList<IMessageThread>();
        }

        public IMessageThread GetByAgentAndRequest(long AgentId, long RequestId)
        {
            return context.MessageThreads.Where(m => m.AgentId == AgentId && m.RequestId == RequestId).FirstOrDefault();
        }
    }
}
