using Core.Log;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
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
    }
}
