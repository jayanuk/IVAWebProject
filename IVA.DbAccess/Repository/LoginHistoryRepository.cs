using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class LoginHistoryRepository : BaseRepository
    {
        public LoginHistoryRepository(AppDBContext context) : base(context)
        {                
        }

        public void Add(long UserId, int ClientType)
        {
            var loginHistory = new LoginHistory
            {
                UserId = UserId,
                ClientType = ClientType,
                Time = DateTime.UtcNow
            };
            context.LoginHistories.Add(loginHistory);
            context.SaveChanges();
        }
    }
}
