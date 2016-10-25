using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class CompanyRepository : BaseRepository
    {
        public CompanyRepository(AppDBContext context) : base(context)
        {
        }

        public List<ICompany> GetAll()
        {
            return context.Companies.Where(c => c.IsActive).ToList<ICompany>();
        }
    }
}
