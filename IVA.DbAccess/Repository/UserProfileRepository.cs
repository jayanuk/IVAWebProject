using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class UserProfileRepository : BaseRepository
    {
        public UserProfileRepository(AppDBContext context) : base(context)
        {

        }
    }
}
