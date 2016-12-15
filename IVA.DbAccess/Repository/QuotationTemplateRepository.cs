using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class QuotationTemplateRepository : BaseRepository
    {
        public QuotationTemplateRepository(AppDBContext context) : base(context)
        {
        }

        public IQuotationTemplate GetById(long Id)
        {
            return context.QuotationTemplates.Where(q => q.Id == Id).FirstOrDefault();
        }

        public List<IQuotationTemplate> GetByCompany(int CompanyId)
        {
            return context.QuotationTemplates.Where(
                q => q.CompanyId == CompanyId).ToList<IQuotationTemplate>();
        }

        public long Add(IQuotationTemplate Template)
        {
            QuotationTemplate template = new QuotationTemplate {
                Name = Template.Name,
                Body = Template.Body,
                ValidityId = Template.ValidityId,
                CompanyId = Template.CompanyId,
                CreatedBy = Template.CreatedBy,
                CreatedDate = DateTime.Now.ToUniversalTime()
            };

            context.QuotationTemplates.Add(template);
            context.SaveChanges();
            return template.Id;
        }

        public void Update(IQuotationTemplate Template)
        {
            QuotationTemplate template = context.QuotationTemplates.Where(q => q.Id == Template.Id).FirstOrDefault();
            if (template != null)
            {
                template.Name = Template.Name;
                template.Body = Template.Body;
                template.ValidityId = Template.ValidityId;
                template.ModifiedBy = Template.ModifiedBy;
                template.ModifiedDate = DateTime.Now.ToUniversalTime();

                context.SaveChanges();
            }
        }
    }
}
