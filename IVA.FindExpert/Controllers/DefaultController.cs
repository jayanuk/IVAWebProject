using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static IVA.Common.Constant;

using Microsoft.AspNet.Identity.Owin;
using IVA.FindExpert.Models;
using Microsoft.AspNet.Identity;

namespace IVA.FindExpert.Controllers.WebAPI
{
    public class DefaultController : BaseController
    {
        //[Authorize]
        [HttpGet]
        public IHttpActionResult Test()
        {
            //ServiceRequestController.AssignRequestToAgents(5);
            return Ok();
        }        
    }
}
