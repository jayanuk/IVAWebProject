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

namespace IVA.FindExpert.Controllers.WebAPI
{
    public class DefaultController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok(new { s = "Hello!"});
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PhoneValidate([FromBody] PhoneValidateRequest request)
        {
            try
            {
                var code = Utiliti.GenerateRandomNumber().ToString();
                IUserPasscode passCode = new UserPasscode();
                passCode.Code = code;
                passCode.Phone = request.Phone;
                passCode.Name = request.Name;

                using (AppDBContext context = new AppDBContext())
                {
                    var repo = new UserPasscodeRepository(context);
                    repo.Add(passCode);
                }

                await Utiliti.SendCode(passCode.Phone, code);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BuyerValidate([FromBody] LoginRequest AppRequest)
        {
            var phone = AppRequest.Phone.Trim();
            var name = AppRequest.Name.Trim();
            bool codeMatch = false;

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
                    return Json(Constant.ErrorCodes.AUTH_ERROR);
                else
                {
                    if (pass.Code.Equals(user.Password))
                        codeMatch = true;
                    else
                        return Json(Constant.ErrorCodes.AUTH_ERROR);
                }

                if(codeMatch)
                {
                    var userRepo = new UserRepository(context);
                    var curUser = userRepo.GetByUserName(user.UserName);
                    user.PasswordValidated = true;

                    if (curUser != null)
                    {
                        user.Id = curUser.Id;
                        user.LoginId = curUser.LoginId;
                        user.CreatedDate = curUser.CreatedDate;
                        user.ModifiedDate = DateTime.Now;
                        userRepo.Update(user);
                    }
                    else
                    { 
                        var loginUser = new ApplicationUser() { UserName = user.UserName };
                        IdentityResult result = await getUserManager().CreateAsync(loginUser, pass.Code);
                        var loginCreated = getUserManager().Find(user.UserName, pass.Code);

                        user.LoginId = loginCreated.Id;
                        user.CreatedDate = DateTime.Now;
                        user.Id = userRepo.Add(user);

                        if (!result.Succeeded)
                        {
                            return InternalServerError();
                        }
                    }
                }                
            }

            return Json(user);
        }

        private ApplicationUserManager getUserManager()
        {
            return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
    }
}
