﻿using IVA.Common;
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
using System.Web.Hosting;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace IVA.FindExpert.Controllers.WebAPI
{
    public class DefaultController : BaseController
    {
        [Authorize]
        [HttpGet]
        public IHttpActionResult Test()
        {
            //ServiceRequestController.AssignRequestToAgents(5);
            //NotificationHelper.GCMNotification("ejV8j1Ife04:APA91bGctCxqnIX3xJmsWrOqnO8b_H8h8L9LFpfXQ_-Eigk-SYQko2h5E6sUge0AHSzPraQzBdQIy7UyH_I90YGB0hnB2E_6h1au_bp0OIrd6fGytuXsPWTnZCjbFDc3-pio7BkpNGbn", "Test Message");
           
            long ServiceRequestId = 1;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var companies = new CompanyRepository(context).GetAll();
                    var request = new ServiceRequestRepository(context).GetById(ServiceRequestId);
                    var agentServiceRepo = new AgentServiceRequestRepository(context);

                    foreach (var company in companies)
                    {
                        //Check vehicle number is recently serviced
                        var agentId = agentServiceRepo.GetAgentIdIfServicedRecently(request.VehicleNo, company.Id);
                        var lastRequestAgent = agentServiceRepo.GetLastOpenServiceAgentIdByCompany(company.Id);

                        if (agentId == 0)
                        {
                            //get agents for each company
                            var agents = new UserRepository(context).GetAgentsByCompany(company.Id);

                            if (agents == null || agents.Count == 0)
                                continue;

                            Dictionary<long, int> counts = new Dictionary<long, int>();

                            foreach (var agent in agents)
                            {
                                //get open requests for each agent
                                var agentRequests = agentServiceRepo.GetByAgentIdForAssign(agent.Id)?.Where(
                                    r => (r.Status ?? 0) != (int)Constant.ServiceRequestStatus.Closed ||
                                        (r.Status ?? 0) != (int)Constant.ServiceRequestStatus.Expired);
                                var weight = 0;
                                if (agent.Id == lastRequestAgent)
                                    weight = 100;

                                if (agentRequests != null)
                                    counts.Add(agent.Id, agentRequests.Count() + weight);
                                else
                                    counts.Add(agent.Id, 0 + weight);
                            }

                            //find agent with min no of requests
                            //order by id asc and get top 1 agent
                            var item = counts.OrderBy(i => i.Value).ThenBy(i => i.Key).First();
                            agentId = item.Key;
                        }

                        //assign the request to agent
                        AgentServiceRequest asr = new AgentServiceRequest
                        {
                            AgentId = agentId,
                            ServiceRequestId = ServiceRequestId,
                            Status = (int)Constant.ServiceRequestStatus.Initial,
                            CreatedTime = DateTime.Now.ToUniversalTime()
                        };

                        agentServiceRepo.Add(asr);

                        new NotificationRepository(context).Add(
                            asr.AgentId,
                            (int)Constant.NotificationType.Request,
                            asr.ServiceRequestId,
                            ConfigurationHelper.NOTIFICATION_TITLE,
                            Constant.Notification.NEW_REQUEST_TEXT);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(DefaultController), ex.Message, LogType.ERROR);
            }

            return Ok();
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

        [HttpGet]
        public HttpResponseMessage GetCompanyLogo(int id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                string logoPath = string.Format("~/Images/CompanyLogo/{0}.jpg", id);
                String filePath = HostingEnvironment.MapPath(logoPath);

                if (!File.Exists(filePath)) //No logo found
                {
                    logoPath = "~/Images/CompanyLogo/NoLogo.jpg";
                    filePath = HostingEnvironment.MapPath(logoPath);
                }

                //FileStream fileStream = new FileStream(filePath, FileMode.Open);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    Image image = Image.FromStream(fileStream);
                    MemoryStream memoryStream = new MemoryStream();
                    image.Save(memoryStream, ImageFormat.Jpeg);
                    result.Content = new ByteArrayContent(memoryStream.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    fileStream.Close();
                }
                    
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(DefaultController), ex.Message, LogType.ERROR);
            }

            return result;
        }

    }
}
