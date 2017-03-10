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
    public class QuotationTemplateController : BaseController
    {

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTemplateById(long Id)
        {
            QuotationTemplateModel model = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var template = new QuotationTemplateRepository(context).GetById(Id);
                    model = new QuotationTemplateModel
                    {
                        Id = template.Id,
                        Name = template.Name,
                        CompanyId = template.CompanyId,
                        Body = template.Body,
                        ValidityId = template.ValidityId
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationTemplateController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(model);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetTemplateByAgent(long AgentId)
        {
            List<QuotationTemplateModel> list = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var user = new UserRepository(context).GetByUserId(AgentId);
                    var companyId = user.CompanyId;
                    var templates = new QuotationTemplateRepository(context).GetByCompany(companyId ?? 0);
                    list = templates.Select(t => new QuotationTemplateModel {
                        Id = t.Id,
                        Name = t.Name,
                        CompanyId = t.CompanyId,
                        Body = t.Body,
                        ValidityId = t.ValidityId                       
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationTemplateController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Update(QuotationTemplateModel Model)
        {
            try
            {
                QuotationTemplate template = new QuotationTemplate();
                template.Id = Model.Id;
                template.Name = Model.Name;
                template.Body = Model.Body;
                template.ModifiedBy = Model.ModifiedBy;
                template.ModifiedDate = DateTime.Now.ToUniversalTime();

                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new QuotationTemplateRepository(context);
                    repo.Update(template);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(typeof(QuotationTemplateController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Add(QuotationTemplateModel Model)
        {
            try
            {
                QuotationTemplate template = new QuotationTemplate();
                //template.Id = Model.Id;
                template.CompanyId = Model.CompanyId;
                template.Name = Model.Name;
                template.Body = Model.Body;
                template.ValidityId = 1;
                template.CreatedBy = Model.ModifiedBy;
                template.CreatedDate = DateTime.Now.ToUniversalTime();

                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new QuotationTemplateRepository(context);
                    repo.Add(template);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(QuotationTemplateController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError(ex);
            }

            return Ok();
        }
    }
}
