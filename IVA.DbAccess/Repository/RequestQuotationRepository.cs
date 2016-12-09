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

        public void Save(RequestQuotation Quotation)
        {
            var existing = GetByRequestAndAgent(Quotation.ServiceRequestId, Quotation.AgentId);
            if (existing == null)
                Add(Quotation);
            else
                Update(Quotation);
        }

        public List<RequestQuotation> GetByRequestAndAgent(long RequestId, long AgentId)
        {
            return context.RequestQuotations.Where(
                r => r.ServiceRequestId == RequestId && r.AgentId == AgentId).ToList();
        }

        public long Add(RequestQuotation Quotation)
        {
            Quotation.CreatedTime = DateTime.Now;
            Quotation.ModifiedTime = DateTime.Now;
            context.RequestQuotations.Add(Quotation);
            context.SaveChanges();
            return Quotation.Id;
        }

        public void Update(RequestQuotation Quotation)
        {
            var existing = context.RequestQuotations.Where(q => q.Id == Quotation.Id).FirstOrDefault();
            if (existing != null)
            {
                existing.ModifiedTime = DateTime.Now;
                existing.Premimum = Quotation.Premimum;
                existing.Cover = Quotation.Cover;
                existing.QuotationTemplateId = Quotation.QuotationTemplateId;

                context.SaveChanges();
            }
        }

    }
}
