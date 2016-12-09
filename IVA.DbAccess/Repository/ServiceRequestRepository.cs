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

        public ServiceRequest GetById(long Id)
        {
            return context.ServiceRequests.Where(r => r.Id == Id).FirstOrDefault();
        }

        public List<IServiceRequest> GetByBuyerId(long Id)
        {
            return context.ServiceRequests.Where(r => r.UserId == Id).ToList<IServiceRequest>();
        }

        public long Add(ServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = new ServiceRequest();
            sr.InsuranceTypeId = ServiceRequestInstance.InsuranceTypeId;
            sr.Code = ServiceRequestInstance.Code;            
            sr.Status = (int)Constant.ServiceRequestStatus.PendingResponse;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;            
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.VehicleValue = ServiceRequestInstance.VehicleValue;
            sr.VehicleYear = ServiceRequestInstance.VehicleYear;
            sr.TimeOccured = DateTime.Now;
            sr.IsFinanced = ServiceRequestInstance.IsFinanced;

            context.ServiceRequests.Add(sr);
            context.SaveChanges();
            UpdateSRCode(sr.Id);
            return sr.Id;
        }

        public void Update(ServiceRequest ServiceRequestInstance)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestInstance.Id).FirstOrDefault();
            if (sr == null)
                return;

            sr.InsuranceTypeId = ServiceRequestInstance.InsuranceTypeId;
            sr.ClaimType = ServiceRequestInstance.ClaimType;
            sr.RegistrationCategory = ServiceRequestInstance.RegistrationCategory;
            sr.UsageType = ServiceRequestInstance.UsageType;
            sr.UserId = ServiceRequestInstance.UserId;
            sr.VehicleNo = ServiceRequestInstance.VehicleNo;
            sr.VehicleValue = ServiceRequestInstance.VehicleValue;
            sr.VehicleYear = ServiceRequestInstance.VehicleYear;
            sr.IsFinanced = ServiceRequestInstance.IsFinanced;

            context.SaveChanges();
        }

        public void UpdateSRStatus(long ServiceRequestId, int Status)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestId).FirstOrDefault();
            if (sr == null)
                return;
            sr.Status = Status;
            context.SaveChanges();
        }

        private void UpdateSRCode(long ServiceRequestId)
        {
            ServiceRequest sr = context.ServiceRequests.Where(r => r.Id == ServiceRequestId).FirstOrDefault();
            sr.Code = "SR-" + String.Format("{0:D6}", sr.Id);
            context.SaveChanges();
            
        }

        public void AddQuotation(RequestQuotation Quotation)
        {
            var existing = context.RequestQuotations.Where(q => q.ServiceRequestId == Quotation.ServiceRequestId).FirstOrDefault();
            if(existing == null)
                context.RequestQuotations.Add(Quotation);
        }
    }
}
