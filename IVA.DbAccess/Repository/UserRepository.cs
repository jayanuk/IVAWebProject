using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(AppDBContext context) : base(context)
        {
        }

        public IUser GetByUserName(string UserName)
        {
            try
            {
                UserName = UserName.Trim();
                return context.Users.Where(u => UserName.Equals(u.UserName)).FirstOrDefault();
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        public IUser GetByUserNameAndPassword(string UserName, string Password)
        {
            UserName = UserName.Trim();
            return context.Users.Where(u => UserName.Equals(u.UserName) && 
                                        u.Password.Equals(Password) && (u.IsActive ?? false)).FirstOrDefault();
        }

        public IUser GetByLoginId(long LoginId)
        {
            return context.Users.Where(u => u.LoginId == LoginId).FirstOrDefault();
        }

        public IUser GetByUserId(long Id)
        {
            return context.Users.Where(u => u.Id == Id).FirstOrDefault();
        }

        public long Add(IUser AppUser)
        {
            User user = new User();
            user.LoginId = AppUser.LoginId;
            user.UserType = AppUser.UserType;
            user.Name = AppUser.Name;
            user.Password = AppUser.Password;
            user.UserName = AppUser.UserName;
            user.PasswordValidated = AppUser.PasswordValidated;            
            user.CompanyId = AppUser.CompanyId;
            user.IsActive = AppUser.IsActive;
            user.CreatedDate = AppUser.CreatedDate;

            context.Users.Add(user);
            context.SaveChanges();
            return user.Id;
        }

        public void Update(IUser AppUser)
        {
            User user = context.Users.Where(u => u.Id == AppUser.Id).FirstOrDefault();
            user.LoginId = AppUser.LoginId;
            user.UserType = AppUser.UserType;
            user.Name = AppUser.Name;
            user.Password = AppUser.Password;
            user.UserName = AppUser.UserName;
            user.PasswordValidated = AppUser.PasswordValidated;
            user.ConnectionId = AppUser.ConnectionId;
            user.CompanyId = AppUser.CompanyId;
            user.IsActive = AppUser.IsActive;
            user.ModifiedDate = AppUser.ModifiedDate;
            context.SaveChanges();            
        }

        public List<IUser> GetAgentsByCompany(int CompanyId)
        {
            return context.Users.Where(
                u => u.CompanyId == CompanyId && (u.IsActive ?? false)).ToList<IUser>();
        }

        public string GetName(long UserId)
        {
            var name = GetByUserId(UserId).Name;
            var profile = new UserProfileRepository(context).GetByUserId(UserId);
            if(profile != null)
            {
                if (!String.IsNullOrEmpty(profile.FirstName))
                    name = profile.FirstName + " " + profile.LastName;
            }
            return name;
        }
    }
}
