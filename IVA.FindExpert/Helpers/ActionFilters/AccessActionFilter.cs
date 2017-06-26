using IVA.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IVA.DbAccess.Repository;

namespace IVA.FindExpert.Helpers.ActionFilters
{
    public class AccessActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var result = 0;
            var userId = HttpContext.Current.User.Identity.GetUserId<long>();
            if(userId > 0)
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var user = new UserRepository(context).GetByLoginId(userId);
                    if (user != null && (user.IsActive ?? false))
                        result = 1;
                }
            }

            if (result == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            base.OnActionExecuting(actionContext);
        }
    }
}