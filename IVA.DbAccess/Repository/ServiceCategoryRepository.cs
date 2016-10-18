using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class ServiceCategoryRepository : BaseRepository
    {
        public ServiceCategoryRepository(AppDBContext context) : base(context)
        {            
        }

        public List<IServiceCategory> GetAll()
        {
            return context.ServiceCategories.Where(c => c.IsActive).ToList<IServiceCategory>();
        }
    }
}
