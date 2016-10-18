using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class ServiceRequestHistoryRepository : BaseRepository
    {
        public ServiceRequestHistoryRepository(AppDBContext context) : base(context)
        {            
        }

        public List<IServiceRequestHistory> GetByServiceRequest(long ServiceRequestId)
        {
            return context.ServiceRequestHistories.Where(
                h => h.ServiceRequestId == ServiceRequestId).ToList<IServiceRequestHistory>();
        }

        public long Add(IServiceRequestHistory HistoryInstance)
        {
            ServiceRequestHistory history = new ServiceRequestHistory();
            history.UserId = HistoryInstance.UserId;
            history.ServiceRequestId = HistoryInstance.ServiceRequestId;
            history.NewValue = HistoryInstance.NewValue;
            history.Oldvalue = HistoryInstance.Oldvalue;
            history.ChangeType = HistoryInstance.ChangeType;
            history.Description = HistoryInstance.Description;
            history.TimeOccured = HistoryInstance.TimeOccured;

            context.ServiceRequestHistories.Add(history);
            context.SaveChanges();
            return history.Id;
        }

        public void AddRange(List<ServiceRequestHistory> Histories)
        {
            context.ServiceRequestHistories.AddRange(Histories);
            context.SaveChanges();
        }

    }
}
