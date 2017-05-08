using Core.Log;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.FindExpert.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class NotificationController : ApiController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetNotifications(long UserId)
        {
            List<Notification> notifications = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new NotificationRepository(context);
                    notifications = repo.GetByUser(UserId);
                    repo.UpdateToRead(UserId);
                }
                return Ok(notifications);
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(NotificationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetNotificationsForList(long UserId, int Page)
        {
            List<Notification> notifications = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new NotificationRepository(context);
                    notifications = repo.GetByUserForList(Page, UserId);
                    if (notifications != null)
                        notifications.ForEach(
                            n => n.DisplayTime = 
                            n.Time.GetAdjustedTime().ToString(Common.Constant.DateFormatType.DateWithTime));
                }
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(NotificationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Delete(NotificationListViewModel Notifications)
        {            
            try
            {                
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new NotificationRepository(context);
                    repo.DeleteList(Notifications.Ids);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(NotificationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult DeleteAll(long UserId)
        {
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new NotificationRepository(context);
                    repo.DeleteAll(UserId);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(NotificationController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }
        }
    }
}
