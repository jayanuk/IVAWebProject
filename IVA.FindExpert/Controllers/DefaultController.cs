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
using IVA.FindExpert.Helpers;
using Core.Log;

namespace IVA.FindExpert.Controllers.WebAPI
{
    public class DefaultController : BaseController
    {
        //[Authorize]
        [HttpGet]
        public IHttpActionResult Test()
        {
            //ServiceRequestController.AssignRequestToAgents(5);
            //NotificationHelper.GCMNotification("ejV8j1Ife04:APA91bGctCxqnIX3xJmsWrOqnO8b_H8h8L9LFpfXQ_-Eigk-SYQko2h5E6sUge0AHSzPraQzBdQIy7UyH_I90YGB0hnB2E_6h1au_bp0OIrd6fGytuXsPWTnZCjbFDc3-pio7BkpNGbn", "Test Message");
            long id = 0;
            using (AppDBContext context = new AppDBContext())
            {
                id = new AgentServiceRequestRepository(context).GetAgentIdIfServicedRecently("wp ke6278", 1);
                Logger.Log(typeof(DefaultController), id.ToString(), LogType.INFO);
            }
            return Ok(id);
        }

        [HttpGet]
        public IHttpActionResult Company()
        {            
            List<ICompany> list = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    list = new CompanyRepository(context).GetAll();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(DefaultController), ex.Message, LogType.ERROR);
            }
            
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetCities(string Text)
        {            
            List<City> list = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    list = new CityRepository(context).GetCities(Text);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(DefaultController), ex.Message, LogType.ERROR);
            }
            
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetCustomerSupportTypes()
        {
            List<ICustomerSupportType> list = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    list = new CustomerSupportTypeRepository(context).GetAll();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(DefaultController), ex.Message, LogType.ERROR);
            }
           
            return Ok(list);
        }
    }
}
