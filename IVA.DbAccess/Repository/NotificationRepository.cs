using IVA.Common;
using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class NotificationRepository : BaseRepository
    {
        public NotificationRepository(AppDBContext context) : base(context)
        {
        }

        public void Add(long RecieverId, int Type, long? RecordId, string Title, string Text)
        {
            Notification message = new Notification();
            message.RecieverId = RecieverId;
            message.Type = Type;
            message.RecordId = RecordId;
            message.Title = Title;
            message.Text = Text;
            message.Time = DateTime.Now;
            message.Status = (int) Constant.MessageStatus.Initial;
            context.Notifications.Add(message);
            context.SaveChanges();
        }

        public void UpdateToRead(long UserId)
        {
            var notifications = context.Notifications.Where(
                n => n.RecieverId == UserId && n.Status == (int)Constant.MessageStatus.Initial).ToList();
            notifications.ForEach(n => n.Status = (int)Constant.MessageStatus.Read);
            context.SaveChanges();
        }

        public List<Notification> GetByUser(long UserId)
        {
            var notifications = context.Notifications.Where(
                n => n.RecieverId == UserId && n.Status == (int)Constant.MessageStatus.Initial
                ).ToList();
            return notifications;
        }
    }
}
