using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public abstract class BaseRepository
    {
        public AppDBContext context;

        public BaseRepository(AppDBContext DBContext)
        {
            this.context = DBContext;
        }
    }
}
