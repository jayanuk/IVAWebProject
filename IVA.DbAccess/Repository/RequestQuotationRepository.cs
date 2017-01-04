using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class RequestQuotationRepository : BaseRepository
    {
        public RequestQuotationRepository(AppDBContext context) : base(context)
        {                
        }

        public RequestQuotation GetById(long Id)
        {
            return context.RequestQuotations.Where(q => q.Id == Id).FirstOrDefault();
        }

        public List<RequestQuotation> GetByRequest(long RequestId)
        {
            return context.RequestQuotations.Include("Agent").Where(
                r => r.ServiceRequestId == RequestId).OrderByDescending(q => q.ModifiedTime).ToList();
        }

        public long Save(RequestQuotation Quotation)
        {
            long id = 0;
            var existing = GetByRequestAndAgent(Quotation.ServiceRequestId, Quotation.AgentId);            
            if (existing == null)
                id = Add(Quotation);
            else
            {
                Quotation.Id = existing.Id;
                Update(Quotation);
                id = existing.Id;
            }
            return id;   
        }

        public RequestQuotation GetByRequestAndAgent(long RequestId, long AgentId)
        {
            return context.RequestQuotations.Where(
                r => r.ServiceRequestId == RequestId && r.AgentId == AgentId).FirstOrDefault();
        }

        public long Add(RequestQuotation Quotation)
        {
            Quotation.CreatedTime = DateTime.Now.ToUniversalTime();
            Quotation.ModifiedTime = DateTime.Now.ToUniversalTime();
            context.RequestQuotations.Add(Quotation);
            context.SaveChanges();
            return Quotation.Id;
        }

        public void Update(RequestQuotation Quotation)
        {
            var existing = context.RequestQuotations.Where(q => q.Id == Quotation.Id).FirstOrDefault();
            if (existing != null)
            {
                existing.ModifiedTime = DateTime.Now.ToUniversalTime();
                existing.Premimum = Quotation.Premimum;
                existing.Cover = Quotation.Cover;
                existing.QuotationTemplateId = Quotation.QuotationTemplateId;

                context.SaveChanges();
            }
        }

    }
}
