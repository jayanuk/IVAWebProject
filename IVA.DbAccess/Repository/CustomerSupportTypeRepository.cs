using IVA.DTO.Contract;
using System.Collections.Generic;
using System.Linq;

namespace IVA.DbAccess.Repository
{
    public class CustomerSupportTypeRepository : BaseRepository
    {
        public CustomerSupportTypeRepository(AppDBContext context) : base(context)
        {
        }

        public List<ICustomerSupportType> GetAll()
        {
            return context.CustomerSupportTypes.Where(x => x.IsActive).ToList<ICustomerSupportType>();
        }
    }
}
