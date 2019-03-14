using System;
using System.Collections.Generic;
using System.Linq;
using MobileReward.Models;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;


namespace MobileReward.ViewModel
{
  public class PromotionViewModel
  {
    public  List<Promotion> Promotions { get; set; }

     MobiRewardDb db = new MobiRewardDb();
    public  List<Promotion> GetAll(bool isActiveOnly = false)
    {
      var q = db.Promotions.OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }

    public List<Promotion> GetAllByPagged(int pageNo, int pageSize, bool isActiveOnly = false)
    {
      var q = db.Promotions.OrderByDescending(x => x.CreatedOn).Skip((pageNo-1)*pageSize).Take(pageSize);
      return q.ToList();
    }

    public  List<Promotion> GetLatest()
    {
      var q = db.Promotions.OrderByDescending(x => x.CreatedOn).Take(5);
      return q.ToList();
    }
    public  List<Promotion> GetIsApproved()
    {
      var q = db.Promotions.Where(x => x.IsApproved == false).OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }
    public List<Promotion> Search(string keyword, int pageNo, int pageSize, string userName = "", bool isActiveOnly = false)
    {
      var q = db.Promotions.Where(x => x.PromotionName.Contains(keyword) || x.PromotionTitle.Contains(keyword) ||
        x.PromotionDescription.Contains(keyword) || x.AdvertisementName.Contains(keyword));

      if (!String.IsNullOrWhiteSpace(userName))
        q = q.Where(x => x.UserProfile.UserName == userName);

      return q.OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
    }

    public List<PromotionsCoupon> SearchCoupon(string keyword, bool isActiveOnly = false)
    {
      var q = db.PromotionsCoupon.Where(x => x.Promotions.UserId.Equals(keyword) || x.CouponTitle.Contains(keyword) ||
        x.CouponDescription.Contains(keyword) || x.Promotions.PromotionName.Contains(keyword));

      return q.OrderByDescending(x => x.CreatedOn).ToList();
    }

    public List<PromotionsCoupon> SearchCouponsInPromotion(int promotionId, string keyword, bool isActiveOnly = false)
    {
      var q = db.PromotionsCoupon.Where(x => x.PromotionId == promotionId && 
        (x.CouponTitle.Contains(keyword) || x.CouponDescription.Contains(keyword) || x.Promotions.PromotionName.Contains(keyword)));

      return q.OrderByDescending(x => x.CreatedOn).ToList();
    }    

    public  List<Promotion> AdminPromotionSearch(string keyword,int pageNo, int pageSize)
    {
      var q = db.Promotions.Where(x => x.PromotionName.Contains(keyword) || x.PromotionTitle.Contains(keyword) ||
        x.PromotionDescription.Contains(keyword) || x.AdvertisementName.Contains(keyword));

      return q.OrderByDescending(x => x.CreatedOn).Skip((pageNo-1)*pageSize).Take(pageSize).ToList();
    }

    public  List<Promotion> GetByUserName(string userName, bool isActiveOnly = false)
    {
      var q = db.Promotions.Include("MasterCountry").Where(x => x.UserProfile.UserName == userName).OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }
    public List<Promotion> GetPaggedByUserName(string userName, int pageNo, int pageSize, bool isActiveOnly = false)
    {
      var q = db.Promotions.Include("MasterCountry").Where(x => x.UserProfile.UserName == userName).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize);
      return q.ToList();
    }
    public  List<Promotion> GetByUserNameApproved(string userName, bool isActiveOnly = false)
    {
      var cuurentTime = DateTime.Now;
      var q = db.Promotions.Include("MasterCountry").Where(x => x.UserProfile.UserName == userName && EntityFunctions.TruncateTime(x.DateEnd) >= EntityFunctions.TruncateTime(cuurentTime) && x.IsApproved == false).OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }

    public  List<PromotionsCoupon> GetByPromotionId(int promotionId)
    {
      var q = db.PromotionsCoupon.Where(x => x.PromotionId == promotionId).OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }
    public List<PromotionsCoupon> GetPaggedByPromotionId(int promotionId)
    {
      var q = db.PromotionsCoupon.Where(x => x.PromotionId == promotionId).OrderByDescending(x => x.CreatedOn);
      return q.ToList();
    }
    public  Promotion Save(Promotion model)
    {
      var p = GetBy(model.PromotionId);

      var user = db.UserProfiles.FirstOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name);

      p.AdvertisementName = model.AdvertisementName;
      p.PromotionTitle = model.PromotionTitle;
      p.PromotionName = model.PromotionName;
      p.PromotionDescription = model.PromotionDescription;
      p.DateStart = model.DateStart;
      p.DateEnd = model.DateEnd;
      p.PaymentReceived = model.PaymentReceived;

      //string barcodeString = user.UserId + "-" + model.PromotionId + "-" + DateTime.Now.ToString() ;
      //p.BarCodeNumber = barcodeString.Replace(" ", "").Replace("/", "-").Replace(":", "-").ToLower();

      //p.RedemptionLimit = model.RedemptionLimit;
      if (model.FilePath == null)
      { }
      else
      {
        p.FilePath = model.FilePath;
      }

      p.TargetCountryId = model.TargetCountryId == 0 ? null : model.TargetCountryId;
      p.IsGlobal = model.IsGlobal;
      p.TargetCountryId = model.IsGlobal == false ? model.TargetCountryId : null;
      p.IsApproved = model.IsApproved;
      p.IsPaid = model.IsPaid;
      p.IsStop = model.IsStop;
      p.UpdateOn = DateTime.Now;
      p.UpdateBy = user.UserId;

      if (p.PromotionId == 0)
      {
        p.Status = true;
        p.UserId = user.UserId;
        p.CreatedBy = user.UserId;
        p.CreatedOn = DateTime.Now;
        db.Promotions.Add(p);
      }


      db.SaveChanges();

      //Update Date start and end to all coupons
      foreach (var item in db.PromotionsCoupon.Where(x => x.PromotionId == p.PromotionId)) 
      {
        item.DateStart = p.DateStart;
        item.DateEnd = p.DateEnd;
      }

      db.SaveChanges();

      return p;
    }

    public void SavePromotionImages(int id, string filePath)
    {
      var p = GetBy(id);
      p.FilePath = filePath;
      db.SaveChanges();
    }

    public  void SaveCouponImages(PromotionsCoupon model)
    {
      var p = GetByCouponId(model.CouponId);
      p.BarCodeImageName = model.BarCodeImageName;
      p.CouponImageName = model.CouponImageName;

      db.SaveChanges();
    }

    public  PromotionsCoupon SaveCoupons(PromotionsCoupon model)
    {
      var pm = db.Promotions.FirstOrDefault(x => x.PromotionId == model.PromotionId);

      var p = GetByCouponId(model.CouponId);

      var user = db.UserProfiles.FirstOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name);

      p.PromotionId = model.PromotionId;
      p.CouponName = model.CouponName;
      p.CouponTitle = model.CouponTitle;

      p.CouponDescription = model.CouponDescription;
      p.TermsAndCondition = model.TermsAndCondition;
      p.DateStart = pm.DateStart;
      p.DateEnd = pm.DateEnd;

      p.RedemptionLimit = model.RedemptionLimit;
      //if (model.CouponImageName == null)
      //{ }
      //else
      //{
      //  p.CouponImageName = model.CouponImageName;
      //}

      //if (model.BarCodeImageName == null)
      //{ }
      //else
      //{
      //  p.BarCodeImageName = model.BarCodeImageName;
      //}

      p.Status = true;

      p.UpdateOn = DateTime.Now;
      p.UpdateBy = user.UserId;

      if (p.CouponId == 0)
      {
        p.CreatedBy = user.UserId;
        p.CreatedOn = DateTime.Now;
        db.PromotionsCoupon.Add(p);
      }

      db.SaveChanges();

      string qrString = user.UserId + "|^|" + p.CouponId + "|^|" + DateTime.Now.ToString() + "|^|" + model.CouponTitle;
      p.QRCode = qrString.Replace(" ", "").Replace("/", "-").Replace(":", "-").ToLower();
      db.SaveChanges();

      return p;
    }

    public  Promotion GetBy(int id)
    {
      var q = db.Promotions.Include("MasterCountry").Where(x => x.PromotionId == id);
      return q.Any() ? q.First() : new Promotion();
    }

    public  PromotionsCoupon GetByCouponId(int id)
    {
      var q = db.PromotionsCoupon.Where(x => x.CouponId == id);
      return q.Any() ? q.First() : new PromotionsCoupon();
    }
  }
}
