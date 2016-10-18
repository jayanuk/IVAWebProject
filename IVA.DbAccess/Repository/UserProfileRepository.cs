using IVA.DTO;
using IVA.DTO.Contract;
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

        public IUserProfile GetByUserId(long UserId)
        {
            return context.UserProfiles.Where(p => p.UserId == UserId).FirstOrDefault();
        }

        public long Add(IUserProfile Profile)
        {
            UserProfile profile = new UserProfile
            {
                UserId = Profile.UserId,
                Phone = Profile.Phone,
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                Email = Profile.Email,
                Image = Profile.Image,
                Location = Profile.Location,
                LocationLatitude = Profile.LocationLatitude,
                LocationLongitude = Profile.LocationLongitude,
                ContactMethod = Profile.ContactMethod,
                NotificationFrequencyMinutes = Profile.NotificationFrequencyMinutes
            };
            context.UserProfiles.Add(profile);
            context.SaveChanges();
            return profile.Id;
        }

        public void Update(IUserProfile Profile)
        {
            UserProfile profile = context.UserProfiles.Where(p => p.Id == Profile.Id).FirstOrDefault();
            profile.FirstName = Profile.FirstName;
            profile.LastName = Profile.LastName;
            profile.Phone = Profile.Phone;
            profile.Email = Profile.Email;
            profile.Image = Profile.Image;
            profile.Location = Profile.Location;
            profile.LocationLatitude = Profile.LocationLatitude;
            profile.LocationLongitude = Profile.LocationLongitude;
            profile.ContactMethod = Profile.ContactMethod;
            profile.NotificationFrequencyMinutes = Profile.NotificationFrequencyMinutes;

            context.SaveChanges();
        }
    }
}
