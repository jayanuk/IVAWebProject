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
                Mobile = Profile.Mobile,
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                Email = Profile.Email,
                Image = Profile.Image,
                Street = Profile.Street,
                City = Profile.City,
                Location = Profile.Location,
                LocationLatitude = Profile.LocationLatitude,
                LocationLongitude = Profile.LocationLongitude,
                ContactMethod = Profile.ContactMethod,
                NotificationFrequencyMinutes = Profile.NotificationFrequencyMinutes,
                BankId = Profile.BankId,
                BankBranch = Profile.BankBranch,
                AccountName = Profile.AccountName,
                AccountNo = Profile.AccountNo
            };
            context.UserProfiles.Add(profile);
            context.SaveChanges();
            return profile.Id;
        }

        public void Update(IUserProfile Profile)
        {
            UserProfile profile = context.UserProfiles.Where(p => p.UserId == Profile.UserId).FirstOrDefault();
            profile.FirstName = Profile.FirstName;
            profile.LastName = Profile.LastName;
            profile.Phone = Profile.Phone;
            profile.Mobile = Profile.Mobile;
            profile.Email = Profile.Email;
            profile.Image = Profile.Image;
            profile.Street = Profile.Street;
            profile.City = Profile.City;
            profile.Location = Profile.Location;
            profile.LocationLatitude = Profile.LocationLatitude;
            profile.LocationLongitude = Profile.LocationLongitude;
            profile.ContactMethod = Profile.ContactMethod;
            profile.NotificationFrequencyMinutes = Profile.NotificationFrequencyMinutes;
            profile.BankId = Profile.BankId == 0 ? null : Profile.BankId;
            profile.BankBranch = Profile.BankBranch;
            profile.AccountNo = Profile.AccountNo;
            profile.AccountName = Profile.AccountName;

            context.SaveChanges();
        }
    }
}
