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
                    curUser = userRepo.GetByUserNameAndPassword(request.Phone, request.Password);

                    if(curUser != null)
                    {
                        var login = GetUserManager().Find(curUser.UserName, curUser.Password);
                        if(login == null)
                        {
                            var loginUser = new ApplicationUser() { UserName = curUser.UserName };
                            IdentityResult result = await GetUserManager().CreateAsync(loginUser, curUser.Password);
                        }
                    }                    
                }                
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return Ok(curUser);
        }

    }
}
