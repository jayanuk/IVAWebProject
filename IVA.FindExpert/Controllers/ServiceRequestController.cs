using IVA.DbAccess;
using IVA.DbAccess.Repository;
using IVA.DTO;
using IVA.DTO.Contract;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class ServiceRequestController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetByBuyerId(long BuyerId)
        {
            var requests = new List<IServiceRequest>();
            using (AppDBContext context = new AppDBContext())
            {
                requests = new ServiceRequestRepository(context).GetByBuyerId(BuyerId);
            }
            return Json(requests);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetBId(long Id)
        {
            IServiceRequest request = null;
            using (AppDBContext context = new AppDBContext())
            {
                request = new ServiceRequestRepository(context).GetById(Id);
            }
            return Json(request);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Save(ServiceRequestModel Model)
        {
            ServiceRequest SR = new ServiceRequest();
            SR.Id = Model.Id;
            SR.Code = Model.Code;
            SR.Description = Model.Description;
            SR.UserId = User.Identity.GetUserId<long>();
            SR.TimeOccured = DateTime.Now;
            SR.ClaimType = Model.ClaimType;
            SR.UsageType = Model.UsageType;
            SR.RegistrationCategory = Model.RegistrationCategory;
            SR.VehicleNo = Model.VehicleNo;
            SR.Vehiclevalue = Model.Vehiclevalue;
            SR.Images = Model.Images;

            using (AppDBContext context = new AppDBContext())
            {
                var repo = new ServiceRequestRepository(context);
                if (SR.Id == 0)
                    SR.Id = repo.Add(SR);
                else
                    repo.Update(SR);

                var imageRepo = new VehicleImageRepository(context);
                imageRepo.Update(SR.Id, SR.Images);
            }

            return Json(SR);
        }
    }
}
