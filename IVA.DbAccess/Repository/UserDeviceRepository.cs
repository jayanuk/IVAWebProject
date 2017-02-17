using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class UserDeviceRepository : BaseRepository
    {
        public UserDeviceRepository(AppDBContext context) : base(context)
        {
        }

        public List<UserDevice> GetByUser(long UserId)
        {
            return context.UserDevices.Where(d => d.UserId == UserId && (d.IsActive ?? false)).ToList();
        }

        public long Add(UserDevice Device)
        {
            var existing = context.UserDevices.Where(d => d.UserId == Device.UserId && d.DeviceId == Device.DeviceId).FirstOrDefault();
            if(existing == null)
            {                
                Device.CreatedDate = DateTime.Now.ToUniversalTime();
                Device.IsActive = true;
                context.UserDevices.Add(Device);
                context.SaveChanges();
                return Device.Id;
            }
            else
            {
                existing.DeviceToken = Device.DeviceToken;
                existing.IsActive = Device.IsActive;
                context.SaveChanges();
                return existing.Id;
            }
        }
    }
}
