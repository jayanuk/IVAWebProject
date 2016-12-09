using IVA.Common;
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
    public class QuotationController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetById(long Id)
        {
            RequestQuotationViewModel model = null;

            using (AppDBContext context = new AppDBContext())
            {
                var quote = new RequestQuotationRepository(context).GetById(Id);
                if (quote != null)
                {
                    model = new RequestQuotationViewModel {
                        Id = quote.Id,
                        ServiceRequestId = quote.ServiceRequestId,
                        QuotationTemplateId = quote.QuotationTemplateId,
                        Premimum = quote.Premimum?.ToString("0.00"),
                        Cover = quote.Cover?.ToString("0.00"),
                        AgentId = quote.AgentId,
                        AgentName = quote.Agent?.Name,
                        AgentContact = quote.Agent?.UserName,
                        CompanyId = quote.Agent?.CompanyId ?? 0,
                        CompanyName = quote.Agent?.Company?.Name,
                        QuotationTemplateName = quote.QuotationTemplate.Name
                    };
                }
            }
            return Ok(model);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Accept(long QuotationId)
        {
            using (AppDBContext context = new AppDBContext())
            {
                var repo = new RequestQuotationRepository(context);
                var quote = repo.GetById(QuotationId);
                quote.Status = (int)Constant.QuotationStatus.Accepted;
                repo.Update(quote);
                var otherQuotes = repo.GetByRequest(quote.ServiceRequestId)?.Where(q => q.Id != quote.Id);
                if (otherQuotes != null)
                {
                    foreach(var q in otherQuotes)
                    {
                        q.Status = (int)Constant.QuotationStatus.Closed;
                        repo.Update(q);
                    }
                }

                var reqRepo = new ServiceRequestRepository(context);
                var request = quote.ServiceRequest;
                request.Status = (int)Constant.ServiceRequestStatus.Closed;
                reqRepo.Update(request);
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Save([FromBody] RequestQuotationViewModel model)
        {
            if (model == null)
                return NotFound();
            var quotation = new RequestQuotation {
                Id = model.Id,
                ServiceRequestId = model.ServiceRequestId,
                AgentId = model.AgentId,
                QuotationTemplateId = model.QuotationTemplateId,
                Premimum = Convert.ToDecimal(model.Premimum),
                Status = (int)Constant.QuotationStatus.Initial,
                Cover = Convert.ToDecimal(model.Cover)
            };

            using (AppDBContext context = new AppDBContext())
            {
                new RequestQuotationRepository(context).Save(quotation);
            }

            return Ok();
        }
    }


}
