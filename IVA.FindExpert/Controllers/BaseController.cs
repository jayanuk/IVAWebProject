using IVA.Common;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class BaseController : ApiController
    {
        public ApplicationUserManager GetUserManager()
        {
            return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }       
    }
}
