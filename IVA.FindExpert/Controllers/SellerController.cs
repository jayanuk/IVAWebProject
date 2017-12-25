using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using IVA.FindExpert.Models;
using Microsoft.AspNet.Identity;
using static IVA.Common.Constant;
using Core.Log;

namespace IVA.FindExpert.Controllers
{
    public class SellerController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SellerValidate([FromBody] LoginRequest request)
        {
            DTO.Contract.IUser curUser = null;
            try
            {                
                using (AppDBContext context = new AppDBContext())
                {
                    var userRepo = new UserRepository(context);
                    curUser = userRepo.GetAgentByUserNameAndPassword(request.Phone, request.Password);

                    if (curUser != null)
                    {
                        var login = GetUserManager().Find(curUser.UserName, curUser.Password);
                        if (login == null)
                        {
                            var loginUser = new ApplicationUser() { UserName = curUser.UserName };
                            IdentityResult result = await GetUserManager().CreateAsync(loginUser, curUser.Password);

                            if (result.Succeeded)
                            {
                                var loginCreated = GetUserManager().Find(curUser.UserName, curUser.Password);

                                if (loginCreated == null)
                                    return null;

                                curUser.LoginId = loginCreated.Id;
                                curUser.CreatedDate = DateTime.Now.ToUniversalTime();
                                curUser.ModifiedDate = DateTime.Now.ToUniversalTime();
                                userRepo.Update(curUser);
                            }
                        }
                        else
                        {
                            curUser.LoginId = login.Id;
                        }
                    }
                    else
                        return null;                                   
                }                
            }
            catch (Exception ex)
            {
                Logger.Log(typeof(SellerController), ex.Message + ex.StackTrace, LogType.ERROR);
                return InternalServerError();
            }

            string token = await Utility.GetToken(curUser.UserName, curUser.Password);

            var model = new UserModel
            {
                Id = curUser.Id,
                LoginId = curUser.LoginId ?? 0,
                UserType = UserType.SELLER,
                Name = curUser.Name,
                Password = curUser.Password,
                PasswordValidated = true,
                Token = token,
                UserName = curUser.UserName,
                CompanyId = curUser.CompanyId ?? 0
            };
            return Ok(model);
        }

    }
}
