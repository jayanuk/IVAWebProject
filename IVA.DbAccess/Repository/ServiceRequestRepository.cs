using IVA.Common;
using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class ServiceRequestRepository : BaseRepository
    {
        public ServiceRequestRepository(AppDBContext context) : base(context)
        {            
        }

        public IServiceRequest GetById(long Id)
        {
            return context.ServiceRequests.Where(r => r.Id == Id).FirstOrDefault();
        }

        public long Add(IServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = new ServiceRequest();
            sr.Code = ServiceRequestInstance.Code;
            sr.Description = ServiceRequestInstance.Description;
            sr.Status = (int)Constant.ServiceRequestStatus.PendingResponse;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;            
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.Vehiclevalue = ServiceRequestInstance.Vehiclevalue;
            sr.TimeOccured = DateTime.Now;

            context.ServiceRequests.Add(sr);
            return sr.Id;
        }

        public void Update(IServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestInstance.Id).FirstOrDefault();
            if (sr == null)
                return;

            sr.Code = ServiceRequestInstance.Code;
            sr.Description = ServiceRequestInstance.Description;
            sr.Status = (int)Constant.ServiceRequestStatus.PendingResponse;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.Vehiclevalue = ServiceRequestInstance.Vehiclevalue;

            context.SaveChanges();
        }
    }
}
