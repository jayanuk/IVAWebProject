using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers.WebAPI
{
    public class DefaultController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok(new { s = "Hello!"});
        }
    }
}
