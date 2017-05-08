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

        public void DeleteList(List<long> IdList)
        {
            foreach(var id in IdList)
            {
                this.Delete(id);
            }
        }

        public void DeleteAll(long UserId)
        {
            var notifications = context.Notifications.Where(n => n.RecieverId == UserId).ToList();
            foreach(var not in notifications)
            {
                this.Delete(not.Id);
            }
        }

        public void Delete(long Id)
        {
            var notification = context.Notifications.Where(n => n.Id == Id).FirstOrDefault();
            if(notification != null)
            {
                notification.IsDeleted = true;
                context.SaveChanges();
            }
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
                && !(n.IsDeleted ?? false)
                ).ToList();
            return notifications;
        }

        public List<Notification> GetByUserForList(int Page, long UserId)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-3);
            var notifications = context.Notifications.Where(
                n => n.RecieverId == UserId && !(n.IsDeleted ?? false) && n.Time > cutOffDate
                ).OrderByDescending(n => n.Time).Skip((Page - 1) * Constant.Paging.NOTIFICATIONS_PER_PAGE).
                       Take(Constant.Paging.NOTIFICATIONS_PER_PAGE).ToList();
            return notifications;
        }
    }
}
