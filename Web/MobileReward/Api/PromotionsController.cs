using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MobileReward.Models;

namespace MobileReward.Api
{
  public class PromotionsController : ApiController
  {
  
    // GET api/default1
    public IList<PromotionPartial> Get()
    {
      var promotions = new List<PromotionPartial>();
      using (var db = new MobiRewardDb())
      {
        foreach (var item in db.Promotions.OrderByDescending(x => x.CreatedOn).ToList())
        {
          var p = new PromotionPartial
            {
              PromotionId = item.PromotionId,
              PromotionTitle = item.PromotionTitle,
              PromotionName = item.PromotionName,
              CountryName = item.MasterCountry != null ? item.MasterCountry.CountryName : "",
              IsGlobal = item.IsGlobal,
              DateStart = item.DateStart,
              DateEnd = item.DateEnd,
              CreatedOn = item.CreatedOn,
              IsPaid = item.IsPaid,
              IsApproved = item.IsApproved
            };

          var coupons = new List<CouponPartial>();
          var cList = db.PromotionsCoupon.Where(x => x.PromotionId == item.PromotionId).ToList();
          p.CouponCounts = cList.Count();
          foreach (var c in cList)
          {
            coupons.Add(new CouponPartial
              {
                CouponId = c.CouponId,
                CouponName = c.CouponName,
                CouponTitle = c.CouponTitle,
                QRCode = c.QRCode,
                RedemptionLimit = c.RedemptionLimit,
                Redeemed = c.Redeemed,
                CouponDescription = c.CouponDescription,
                Status = c.Status,
              });
          }
          p.Coupons = coupons;
          promotions.Add(p);
        }
      }
      return promotions ;
    }

    // GET api/default1/5
    public string Get(int id)
    {
      return "value";
    }

    // POST api/default1
    public void Post([FromBody]string value)
    {
    }

    // PUT api/default1/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/default1/5
    public void Delete(int id)
    {
    }
  }
}
