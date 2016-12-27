using IVA.Common;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class PromotionRepository : BaseRepository
    {
        public PromotionRepository(AppDBContext context) : base(context)
        {                
        }

        public List<IPromotion> GetPromotions(int InsuranceType)
        {
            return context.Promotions.Where(
                p => p.Status == Constant.PromotionStatus.ACTIVE && p.InsuranceType == InsuranceType).
                OrderByDescending(p => p.CreatedDate).ToList<IPromotion>();
        }

        public IPromotion GetLatestPromotion(int InsuranceType)
        {
            return context.Promotions.Where(
                p => p.Status == Constant.PromotionStatus.ACTIVE && p.InsuranceType == InsuranceType).
                OrderByDescending(p => p.CreatedDate).FirstOrDefault();
        }

        public IPromotion GetPromotionById(long Id)
        {
            return context.Promotions.Where(p => p.Id == Id).FirstOrDefault();
        }
    }
}
