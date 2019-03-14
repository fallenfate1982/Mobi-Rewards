using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Http;
using MobileReward.Models;
using WebMatrix.WebData;
using System.Collections.Generic;
using System.Web.Security;
using Postal;
using MobileReward.ViewModel;
using System.Data.Objects;

namespace MobileReward.Api
{
  public class MobiRewardServicesController : ApiController
  {


    PromotionViewModel PromotionViewModel = new PromotionViewModel();
    /// <summary>
    /// method for user login
    /// </summary>
    /// <param name="userName">userName</param>
    /// <param name="password">password</param>
    /// <returns></returns>
    [HttpGet]
    public bool Login(string userName, string password)
    {
      try
      {
        return WebSecurity.Login(userName, password);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// method for checking the validity
    /// </summary>
    /// <param name="qrcode">qrcode</param>
    /// <returns></returns>
    [HttpGet]
    public bool IsValidCoupon(string qrcode)
    {

      try
      {
        qrcode = qrcode.StartsWith("'") ? qrcode.Remove(0, 1) : qrcode;
        qrcode = qrcode.EndsWith("'") ? qrcode.Remove(qrcode.Length - 1, 1) : qrcode;

        var db = new MobiRewardDb();
        var coupon = db.PromotionsCoupon.Where(x => x.QRCode == qrcode);

        if (coupon.Any())
        {
          var p = coupon.First();
          //var promotion = PromotionViewModel.GetByPromotionId(p.PromotionId);
          // var s = promotion.First();

          if (p.RedemptionLimit == p.Redeemed
            || DateTime.Now.Date.Date > p.Promotions.DateEnd.Date
            || DateTime.Now.Date.Date < p.Promotions.DateStart.Date
            || p.Status == false
            || p.Promotions.Status == false
            || p.Promotions.IsStop == true
            || p.Promotions.IsApproved == false)
          {
            return false;
          }
          else
          {
            p.Redeemed = p.Redeemed + 1;
            db.SaveChanges();
            return true;
          }
        }
      }
      catch (Exception)
      {
        throw;
      }

      return false;
    }

    [HttpGet]
    public DateTime GetUpdateOn()
    {
      try
      {
        var db = new MobiRewardDb();
        return db.ApplicationSettings.OrderByDescending(x => x.UpdateOn).FirstOrDefault().UpdateOn;
      }
      catch (Exception)
      {
        throw new Exception("Last Updated Date not found");
      }
    }

    [HttpGet]
    public List<MasterCountry> GetAllCountry()
    {
      var db = new MobiRewardDb();
      return db.MasterCountries.OrderBy(x => x.CountryName).ToList();
    }

    [HttpGet]
    public Promotion GetPromotionById(int promotionId)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.Promotions.FirstOrDefault(x => x.PromotionId == promotionId);
      }
      catch (Exception)
      {
        throw new Exception("Promotion not found!");
      }
    }


    /// <summary>
    /// method for get list for all prootions
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public List<Promotion> GetAllPromotions()
    {
      try
      {
        var db = new MobiRewardDb();
        return db.Promotions.Where(x => x.Status).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public List<Promotion> GetPromotionsByIds(string ids)
    {
      try
      {
        var idList = new List<int>();
        foreach (var item in ids.Split('|'))
        {
          int id;
          if (int.TryParse(item, out id))
          {
            idList.Add(id);
          }
        }
        var db = new MobiRewardDb();
        return db.Promotions.Where(x => idList.Contains(x.PromotionId)).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public Promotion GetPromotionsById(int id)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.Promotions.FirstOrDefault(x => x.PromotionId == id);
      }
      catch (Exception)
      {
        throw;
      }
    }

    private byte[] GetBytesFromImage(String imageFile)
    {
      try
      {
        var ms = new MemoryStream();
        var img = Image.FromFile(imageFile);
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

        return ms.ToArray();
      }
      catch (Exception)
      {

        return null;
      }
    }

    [HttpGet]
    public List<PromotionsCoupon> GetCouponsByPromotionId(int id)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.PromotionsCoupon.Where(x => x.PromotionId == id
                                              && x.Status == true
                                              && x.RedemptionLimit >= x.Redeemed
                                              && EntityFunctions.TruncateTime(x.Promotions.DateEnd) >= EntityFunctions.TruncateTime(DateTime.Now)
                                              && x.Promotions.IsApproved
                                              && x.Promotions.IsStop == false
                                              && x.Promotions.Status).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public List<CouponDetail> GetCouponsByIds(string ids)
    {
      var couponList = new List<CouponDetail>();
      try
      {
        var idList = new List<int>();
        foreach (var item in ids.Split('|'))
        {
          int id;
          if (int.TryParse(item, out id))
          {
            idList.Add(id);
          }
        }
        var db = new MobiRewardDb();

        foreach (var item in db.PromotionsCoupon.Where(x => idList.Contains(x.CouponId)).ToList())
        {
          var coupon = new CouponDetail
            {
              CouponId = item.CouponId,
              PromotionId = item.PromotionId,
              PromotionName = item.Promotions.PromotionName,
              CouponName = item.CouponName,
              CouponTitle = item.CouponTitle,
              QRCode = item.QRCode,
              BarCodeImageName = item.BarCodeImageName,
              CouponImageName = item.CouponImageName,
              RedemptionLimit = item.RedemptionLimit,
              Redeemed = item.Redeemed,
              CouponDescription = item.CouponDescription,
              TermsAndCondition = item.TermsAndCondition,
              DateStart = item.Promotions.DateStart,
              DateEnd = item.Promotions.DateEnd,
              Status = item.Status,
              CreatedBy = item.CreatedBy,
              CreatedOn = item.CreatedOn,
              UpdateBy = item.UpdateBy,
              UpdateOn = item.UpdateOn
            };

          string userFile = System.Web.HttpContext.Current.Server.MapPath("~/UserFiles/");
          coupon.CouponImage = GetBytesFromImage(userFile + "Promotions/Coupons/" + coupon.CouponImageName);
          coupon.BarCodeImage = GetBytesFromImage(userFile + "BarCodes/" + coupon.BarCodeImageName);
          coupon.QRCodeImage = GetBytesFromImage(userFile + "Promotions/Coupons/QRCodes/" + "QR_" + coupon.CouponId + ".png");
          couponList.Add(coupon);
        }
      }
      catch (Exception)
      {
        throw;
      }

      return couponList;
    }

    [HttpGet]
    public PromotionsCoupon GetCouponById(int id)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.PromotionsCoupon.FirstOrDefault(x => x.CouponId == id);

      }
      catch (Exception)
      {
        throw;
      }
    }


    [HttpGet]
    public List<Promotion> GetPromotionsByMerchantId(int merchantId, string promotionName, string endDate, int countryId)
    {
      try
      {
        var db = new MobiRewardDb();
        var q = db.Promotions.Where(x => x.Status && x.CreatedBy == merchantId);

        if (!String.IsNullOrWhiteSpace(promotionName))
        {
          q = q.Where(x => (x.PromotionName.Contains(promotionName) || x.PromotionTitle.Contains(promotionName))
                           && x.TargetCountryId == countryId);
        }

        if (!String.IsNullOrWhiteSpace(endDate))
        {

          var currentDate = DateTime.Now;
          var tempEndDate = ParseDate(endDate);
          q = q.Where(x => x.DateEnd >= tempEndDate && x.DateEnd >= currentDate);
        }

        return q.ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public List<Promotion> PromotionSearch(string promotionName, string endDate, string browseBy, int countryId)
    {
      try
      {
        var db = new MobiRewardDb();
        var q = db.Promotions.Where(x => x.Status && db.PromotionsCoupon.Any(c => c.PromotionId == x.PromotionId));

        if (!String.IsNullOrWhiteSpace(promotionName))
        {
          q = q.Where(x => (x.PromotionName.Contains(promotionName) || x.PromotionTitle.Contains(promotionName)));
        }

        var currentDate = DateTime.Now;

        if (!String.IsNullOrWhiteSpace(endDate))
        {
          var tempEndDate = ParseDate(endDate);
          q = q.Where(x => (EntityFunctions.TruncateTime(x.DateStart) >= EntityFunctions.TruncateTime(currentDate) && EntityFunctions.TruncateTime(x.DateStart) <= EntityFunctions.TruncateTime(tempEndDate)) || (EntityFunctions.TruncateTime(x.DateEnd) >= EntityFunctions.TruncateTime(currentDate) && EntityFunctions.TruncateTime(x.DateEnd) <= EntityFunctions.TruncateTime(tempEndDate)));
        }
        else
        {
          q = q.Where(x => (EntityFunctions.TruncateTime(x.DateEnd) >= EntityFunctions.TruncateTime(currentDate)));
        }

        var p = q.Where(x => (x.TargetCountryId == countryId || x.IsGlobal) && x.IsApproved).ToList();
        var promotions = p.ToList();

        foreach (var item in promotions)
        {
          var coupons = db.PromotionsCoupon.Where(x => x.PromotionId == item.PromotionId && x.Status);
          if (coupons.Count() == 1)
          {
            item.IsCoupon = true;
            var coupon = coupons.First();
            item.CoupanDetails = new CouponDetail
            {
              CouponId = coupon.CouponId,
              CouponDescription = coupon.CouponDescription,
              BarCodeImageName = coupon.BarCodeImageName,
              CouponImageName = coupon.CouponImageName,
              CouponName = coupon.CouponName,
              CouponTitle = coupon.CouponTitle,
              QRCode = coupon.QRCode,
              Redeemed = coupon.Redeemed,
              RedemptionLimit = coupon.RedemptionLimit,
              Status = coupon.Status,
              TermsAndCondition = coupon.TermsAndCondition,
              UpdateBy = coupon.UpdateBy,
              UpdateOn = coupon.UpdateOn
            };
          }
        }

        return p.ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public List<Promotion> BrowserAllPromotions(int countryId)
    {
      try
      {
        var db = new MobiRewardDb();
        var q = db.Promotions.Where(x => x.Status);

        var currentDate = DateTime.Now.Date.Date;
        q = q.Where(x => (x.TargetCountryId == countryId || x.IsGlobal)
                            && EntityFunctions.TruncateTime(x.DateEnd) >= EntityFunctions.TruncateTime(currentDate)
                            && db.PromotionsCoupon.Any(c => c.PromotionId == x.PromotionId
                            && x.IsApproved)
                          );
        return q.ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    private DateTime ParseDate(string endDateCode)
    {
      var endDate = new DateTime();
      switch (endDateCode)
      {
        case "1":
          endDate = DateTime.Now.AddDays(7);
          break;
        case "2":
          endDate = DateTime.Now.AddDays(14);
          break;
        case "3":
          endDate = DateTime.Now.AddDays(21);
          break;
        case "4":
          endDate = DateTime.Now.AddMonths(1);
          break;
        case "5":
          endDate = DateTime.Now.AddMonths(3);
          break;
        case "6":
          endDate = DateTime.Now.AddMonths(6);
          break;
      }

      return endDate;
    }

    /// <summary>
    /// method for get list for all prootions according to keyword
    /// </summary>
    /// <param name="keywords">keywords</param>
    /// <returns></returns>


    [HttpGet]
    public List<Promotion> GetPromotionsByKeywords(string keywords)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.Promotions.Where(x => x.Status == true &&
                                    (
                                      x.PromotionName.Contains(keywords)
                                      || x.PromotionTitle.Contains(keywords))
                                    ).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// method for get list for all prootions according to end date
    /// </summary>
    /// <param name="endDate">endDate</param>
    /// <returns></returns>

    [HttpGet]
    public List<Promotion> GetPromotionsByEndDate(string endDate)
    {
      try
      {
        DateTime date;
        if (DateTime.TryParse(endDate, out date))
        {
          var db = new MobiRewardDb();
          return db.Promotions.Where(x => x.Status && x.DateEnd >= date).ToList();
        }
        else
        {
          throw new Exception("Date format is invalide");
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// method for get list for all prootions according to country name
    /// </summary>
    /// <param name="countryName">countryName</param>
    /// <returns></returns>

    [HttpGet]
    public List<Promotion> GetPromotionsByCountryName(string countryName)
    {
      try
      {
        var db = new MobiRewardDb();
        return db.Promotions.Where(x => x.Status && x.MasterCountry.CountryName.Contains(countryName)).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet]
    public bool ForgetPassword(string userName)
    {
      try
      {

        var db = new MobiRewardDb();
        var user = db.UserProfiles.SingleOrDefault(x => x.UserName == userName || x.Email ==  userName);
        if (user != null && user.Status)
        {
          string emailAddress = user.Email;
          if (!string.IsNullOrEmpty(emailAddress))
          {
            string confirmationToken = WebSecurity.GeneratePasswordResetToken(user.UserName);
            dynamic email = new Email("ForgetEmail");
            email.To = emailAddress;
            email.UserName = user.UserName;
            email.ConfirmationToken = confirmationToken;
            email.Send();
            return true;
          }
        }
      }
      catch (Exception ex)
      {
      }

      return false;
    }

    /// <summary>
    /// method for get list for all prootions according to promotion id
    /// </summary>
    /// <param name="promotionId">promotionId</param>
    /// <returns></returns>

    //[HttpGet]
    //public bool RedeemCoupon(int promotionId)
    //{
    //  bool status = false;
    //  try
    //  {
    //    var db = new MobiRewardDb();
    //    var promotion = db.Promotions.Where(x => x.PromotionId == promotionId);
    //    if (promotion.Any())
    //    {
    //      var p = promotion.First();
    //      /*if (p.RedemptionLimit == p.Grabbed)
    //      {
    //        throw new Exception("Coupon redemption limit exceeded!");
    //      }
    //      else
    //      {
    //        p.Grabbed = p.Grabbed + 1;
    //        db.SaveChanges();
    //        status = true;
    //      }*/
    //    }
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }

    //  return status;
    //}

    /// <summary>
    /// method for save the detail of merchant
    /// </summary>
    /// <param name="userName">userName</param>
    /// <param name="email">email</param>
    /// <param name="password">password</param>
    /// <param name="companyRegNo">companyRegNo</param>
    /// <param name="address">address</param>
    /// <param name="conactNo">conactNo</param>
    /// <returns></returns>

    [HttpGet]
    public bool RegisterMerachant(string userName, string email, string password,
                     string companyRegNo, string address, string conactNo)
    {
      try
      {
        if (WebSecurity.UserExists(userName))
          throw new Exception("User name already exist!");

        var db = new MobiRewardDb();
        var user = db.UserProfiles.Where(x => x.Email == email);
        if (user.Any())
          throw new Exception("Email already exist!");

        string confirmationToken =
          WebSecurity.CreateUserAndAccount(userName, password,
                                           new
                                           {
                                             Email = email,
                                             CompanyRegistrationNo = companyRegNo,
                                             Address = address,
                                             ContactNo = conactNo
                                           },
                                           true);

        //Assign Role as Merchant
        Roles.AddUsersToRole(new[] { userName }, "Merchant");

        //Send confirmation email
        dynamic pEmail = new Email("RegEmail");
        pEmail.To = email;
        pEmail.UserName = userName;
        pEmail.ConfirmationToken = confirmationToken;
        pEmail.Send();
      }
      catch (Exception)
      {
        throw;
      }

      return true;
    }

  }
}
