using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static IVA.Common.Constant;

using Microsoft.AspNet.Identity.Owin;
using IVA.FindExpert.Models;
using Microsoft.AspNet.Identity;

namespace IVA.FindExpert.Controllers
{
    public class BuyerController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PhoneValidate([FromBody] PhoneValidateRequest request)
        {
            try
            {
                var code = Utility.GenerateRandomNumber().ToString();
                IUserPasscode passCode = new UserPasscode();
                passCode.Code = code;
                passCode.Phone = request.Phone;
                passCode.Name = request.Name;

                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new UserPasscodeRepository(context);
                    repo.Add(passCode);
                }

                await Utility.SendCode(passCode.Phone, code);
            }
            catch (Exception ex)
            {                
                return InternalServerError();
            }

            return Ok("SUCCESS");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CodeValidate([FromBody] LoginRequest AppRequest)
        {
            var phone = AppRequest.Phone.Trim();
            var name = AppRequest.Name.Trim();
            bool codeMatch = false;
            string token = String.Empty;

            IUserProfile profile = null;
            DTO.Contract.IUser user = new User();
            user.UserType = UserType.BUYER;
            user.Name = name;
            user.UserName = phone;
            user.Password = AppRequest.Password;
            user.PasswordValidated = false;

            using (AppDBContext context = new AppDBContext())
            {
                //validate code                
                var pass = new UserPasscodeRepository(context).GetByPhone(phone);
                if (pass == null)
                    return Unauthorized();
                else
                {
                    if (pass.Code.Equals(user.Password))
                        codeMatch = true;
                    else
                        return Unauthorized();
                }

                if (codeMatch)
                {
                    var userRepo = new UserRepository(context);
                    var curUser = userRepo.GetByUserName(user.UserName);
                    user.PasswordValidated = true;

                    if (curUser != null)
                    {
                        if (!(curUser.IsActive ?? false))
                            return Unauthorized();

                        if (curUser.LoginId != null)
                            await GetUserManager().ChangePasswordAsync(curUser.LoginId ?? 0, curUser.Password, pass.Code);                            

                        user.Id = curUser.Id;
                        user.LoginId = curUser.LoginId;
                        user.Password = pass.Code;
                        user.CreatedDate = curUser.CreatedDate;
                        user.ModifiedDate = DateTime.Now;
                        userRepo.Update(user);
                    }
                    else
                    {
                        var loginUser = new ApplicationUser() { UserName = user.UserName };
                        IdentityResult result = await GetUserManager().CreateAsync(loginUser, pass.Code);
                        var loginCreated = GetUserManager().Find(user.UserName, pass.Code);

                        if (loginCreated == null)
                            return null;

                        user.LoginId = loginCreated.Id;
                        user.CreatedDate = DateTime.Now;
                        user.Id = userRepo.Add(user);

                        if (!result.Succeeded)
                        {
                            return InternalServerError();
                        }                        
                    }

                    profile = new UserProfileRepository(context).GetByUserId(user.Id);
                }
            }

            token = await Utility.GetToken(user.UserName, user.Password);
            
            var model = new UserModel {
                                        Id = user.Id,
                                        LoginId = user.LoginId ?? 0,
                                        UserType = UserType.BUYER,
                                        Name = user.Name,
                                        Password =  user.Password,
                                        PasswordValidated = true,
                                        Token = token,
                                        UserName = user.UserName
            };
            if(profile != null)
            {
                UserProfileModel userProfileModel = new UserProfileModel
                {
                    Id = profile.Id,
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Gender = profile.Gender ?? 0,
                    Email = profile.Email,
                    Phone = profile.Email,
                    Mobile = profile.Mobile,
                    Street = profile.Street,
                    City = profile.City,
                    Image = profile.Image,
                    Location = profile.Location,
                    ContactMethod = profile.ContactMethod ?? 0,
                    BankId = profile.BankId,
                    BankBranch = profile.BankBranch,
                    AccountName = profile.AccountName,
                    AccountNo = profile.AccountNo,
                    NotificationFrequencyMinutes = profile.NotificationFrequencyMinutes ?? 0
                };
                model.UserProfile = userProfileModel;
            }
            return Ok(model);
        }        
    }
}
