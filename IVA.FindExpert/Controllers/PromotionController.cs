using IVA.Common;
using IVA.DbAccess;
using IVA.DbAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IVA.FindExpert.Controllers
{
    public class PromotionController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPromotions(int Type)
        {
            List<PromotionModel> promotions = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    promotions = new PromotionRepository(context).GetPromotions(Type)?.Select(
                        p => new PromotionModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Header = p.Header,
                            Description = p.Description,
                            CreatedDate = p.CreatedDate?.ToString(Constant.DateFormatType.YYYYMMDD),
                            Status = p.Status ?? 0,
                            Type = ((p.Type ?? 0) == Constant.PromotionType.OFFER) ? "Offers" : "Promotions"
                        }).ToList();
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(promotions);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPromotionById(int Id)
        {
            PromotionModel promotion = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var p = new PromotionRepository(context).GetPromotionById(Id);
                    promotion = new PromotionModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Header = p.Header,
                        Description = p.Description,
                        CreatedDate = p.CreatedDate?.ToString(Constant.DateFormatType.YYYYMMDD),
                        Status = p.Status ?? 0,
                        Type = ((p.Type ?? 0) == Constant.PromotionType.OFFER) ? "Offers" : "Promotions"
                    };
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(promotion);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetLatestPromotion(int Type)
        {
            PromotionModel promotion = null;
            try
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var p = new PromotionRepository(context).GetLatestPromotion(Type);
                    promotion = new PromotionModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Header = p.Header,
                        Description = p.Description,
                        CreatedDate = p.CreatedDate?.ToString(Constant.DateFormatType.YYYYMMDD),
                        Status = p.Status ?? 0,
                        Type = ((p.Type ?? 0) == Constant.PromotionType.OFFER) ? "Offers" : "Promotions"
                    };
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(promotion);
        }
    }
}
