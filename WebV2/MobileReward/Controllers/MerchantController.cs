using System;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobiReward.Common;
using MobileReward.Models;
using MobileReward.ViewModel;
using System.Net;
using WebMatrix.WebData;
using System.Web.Security;
using Postal;
using System.Collections.Generic;
using System.Configuration;

namespace MobileReward.Controllers
{
  [Authorize(Roles = ("Merchant, Admin,User"))]
  //[System.Web.Http.Authorize(Roles = ("Merchant"))]
  public class MerchantController : Controller
  {
    MobiRewardDb db = new MobiRewardDb();
    PromotionViewModel PromotionViewModel = new PromotionViewModel();
    const int pageSize = 10;
    public ActionResult PromotionList(int page = 1)
    {
      var list = PromotionViewModel.GetPaggedByUserName(User.Identity.Name, page, pageSize);
      ViewBag.CurrentPage = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = Math.Ceiling((double)PromotionViewModel.GetByUserName(User.Identity.Name).Count() / pageSize);
      return View(list);
    }

    [HttpPost]
    public ActionResult PromotionList(string keyword, string command, int page = 1)
    {
      var list = new List<Promotion>();
      if (command == "Reset")
      {
        list = PromotionViewModel.GetPaggedByUserName(User.Identity.Name, page, pageSize);
        ViewBag.TotalPages = Math.Ceiling((double)PromotionViewModel.GetByUserName(User.Identity.Name).Count() / pageSize);
      }
      else
      {
        list = PromotionViewModel.Search(keyword, page, pageSize, User.Identity.Name);
       
        ViewBag.TotalPages = Math.Ceiling((double)list.Count() / pageSize);
      }
      ViewBag.CurrentPage = page;
      ViewBag.PageSize = pageSize;
    
      return View(list);
    }

    public ActionResult PromotionDetails(int promotionid)
    {

      var p = PromotionViewModel.GetBy(promotionid);
      ViewBag.ShowNotes = !db.PromotionsCoupon.Any(x => x.PromotionId == promotionid);

      return View(p);
    }

    public ActionResult PromotionEdit(int promotionId = 0)
    {
      
      var promotion = db.Promotions.FirstOrDefault(x => x.PromotionId == promotionId);
      promotion = promotion ?? new Promotion();
      int defaultCountryId;
      int.TryParse(ConfigurationManager.AppSettings["DefaultCountryId"], out defaultCountryId);

      PopulateCountryDropDownList(defaultCountryId);
      PopulateStateDropDownList();

      if (promotionId != 0)
      {
        ViewBag.promotionID = promotionId;        
      }
      else
      {
        promotion.DateEnd  =promotion.DateStart = DateTime.Now;
      }

      return View(promotion);
    }
    public ActionResult Dashboard()
    {
      var list = PromotionViewModel.GetByUserName(User.Identity.Name);
      //var promotion = db.Promotions.FirstOrDefault(x => x.Status == true);
      //var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      //ViewData["userName"] = user.FirstName + " " + user.LastName;
      return View(list);
    }

    public ActionResult LatestPromotions()
    {
      var list = PromotionViewModel.GetByUserName(User.Identity.Name);
      //var promotion = db.Promotions.FirstOrDefault(x => x.Status == true);
      //var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      //ViewData["userName"] = user.FirstName + " " + user.LastName;
      return View(list);
    }

    [HttpPost]
    public ActionResult PromotionEdit(Promotion model, HttpPostedFileBase photoFile)
    {
      model.DateStart = model.DateStart == DateTime.MinValue ? DateTime.Now : model.DateStart;
      model.DateEnd = model.DateEnd == DateTime.MinValue ? DateTime.Now : model.DateEnd;
      var promotion = new Promotion();
      var data = db.Promotions.Where(x => x.PromotionId == model.PromotionId).FirstOrDefault();
      PopulateCountryDropDownList();

      if (data!=null)
     {
       if (data.IsApproved == true || data.IsStop == true)
         {
           TempData["ErrorNotification"] = "This promotion has been stopped or already approved.";
          // ModelState.AddModelError("Not Approved", "This promotion has been already approved");
           //return View(model);
           return RedirectToAction("promotionlist");
         }
        //if(data.IsStop == true)
        //{
        //  TempData["ErrorNotification"] = "This promotion has been stop or already approved.";
        //  return RedirectToAction("promotionlist");
        //}
     }
      if (model.IsGlobal == false && model.TargetCountryId == null)
      {
        ModelState.AddModelError("", "Please select Country.");
        return View(model);
      }


      if (model.DateEnd >= model.DateStart)
      {
        model.IsStop = model.IsStop;
        if (ModelState.IsValid)
        {
          promotion = PromotionViewModel.Save(model);
          if (Request.Files.Count > 0)
          {
            try
            {
              var file = Request.Files[0];

              if (file.InputStream.Length >= 0)
              {
                if (file.ContentLength > 4194304)
                {
                  ModelState.AddModelError("file", "The size of the file should not exceed 4 MB");
                  return View(model);
                }

                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                  ModelState.AddModelError("file", "Invalid file type, only the following types (jpg, jpeg,png, gif) are supported.");
                  return View(model);
                }

                string fileName = model.PromotionId + "-" + TextHelper.GenerateRandomText(10) + ".png";
                var source = new Bitmap(file.InputStream);
                var origin = ImageHelper.GetScaledPicture(source, 210, 156);
                var serverPath = Server.MapPath("~/Userfiles/Promotions/" + fileName);
                origin.Save(serverPath);
                model.FilePath = fileName;
                PromotionViewModel.SavePromotionImages(promotion.PromotionId, fileName);
              }
            }

            catch (Exception ex)
            {
              //TODO : Log exceptino
            }

          }
                   

          //TODO: Send notification to admin
          if (model.PromotionId == 0)
          {
            try
            {
              var admin = db.UserProfiles.FirstOrDefault(x => x.UserName == "admin");
              var adminMail = admin.Email;
              var name = admin.FirstName + " " + admin.LastName;
              if (admin != null)
              {
                //Send confirmation email
                dynamic email = new Email("PromotionDetail");
                email.To = adminMail;
                email.MerchantName = name;
                email.PromotionName = model.PromotionName;
                email.Title = model.PromotionTitle;
                email.StartDate = String.Format("{0:M/d/yyyy}", model.DateStart); // model.DateStart;
                email.EndDate = String.Format("{0:M/d/yyyy}", model.DateEnd);// model.DateEnd;
                email.Send();
              }
            }
            catch { }
          }
          TempData["Notification"] = "Promotion saved successfully!";


          if (Request.Form["edit"] != null)
          {
            return RedirectToAction("promotionlist");
          }

          return RedirectToAction("PromotionCouponsEdit", "Merchant", new { promotionId = promotion.PromotionId });
        }
        else
        {
          ModelState.AddModelError("", "");
          return View(model);
        }
      }
      else
      {
        ModelState.AddModelError("", "End date should be greater than start date.");
        return View(model);
      }
    }

    [AllowAnonymous]
    public ActionResult PromotionCouponList(int promotionId = 0)
    {
      var list = PromotionViewModel.GetByPromotionId(promotionId);
      ViewBag.PromotionId = promotionId;
      return View(list);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult PromotionCouponList(int? promotionId, string keyword, string command)
    {
      var id = promotionId ?? 0;  
      var list = new List<PromotionsCoupon>();

      if (command == "Reset")
      {
        list = PromotionViewModel.GetByPromotionId(id);
      }
      else
      {
        list = PromotionViewModel.SearchCouponsInPromotion(id, keyword, true);
      }

      ViewBag.PromotionId = promotionId;
      return View(list);
    }

    [AllowAnonymous]
    public ActionResult PromotionCouponDetails(int couponid)
    {
      var p = PromotionViewModel.GetByCouponId(couponid);
      return View(p);
    }

    public ActionResult PromotionCouponsEdit(int couponid = 0, int? promotionId = 0)
    {
      var coupon = db.PromotionsCoupon.FirstOrDefault(x => x.CouponId == couponid);
      if (promotionId != 0)
      {
        ViewBag.PromotionId = promotionId;
      }

      if (coupon == null)
      {
        coupon = new PromotionsCoupon();
        coupon.DateStart = DateTime.Now;
        coupon.DateEnd = DateTime.Now;
      }


    PopulateApprovedPromotion();

      return View(coupon);
    }

    [HttpPost]
    public ActionResult PromotionCouponsEdit(PromotionsCoupon model, HttpPostedFileBase couponFiles, HttpPostedFileBase barcodeFiles)
    {
      var coupon = new PromotionsCoupon();
      PopulateApprovedPromotion();
      var data = db.PromotionsCoupon.Where(x => x.PromotionId == model.PromotionId).FirstOrDefault();
      var promotion= db.Promotions.Where(x=>x.PromotionId == model.PromotionId).FirstOrDefault();
      if (data != null)
      {
        if (promotion != null && (promotion.IsApproved == true || promotion.IsStop == true))
        {
          TempData["ErrorNotification"] = "Promotion has  been stopped or approved for this coupon. You can not make changes.";
          return RedirectToAction("PromotionCouponList", "Merchant", new { promotionid = model.PromotionId });
        }
      }
      if (!ModelState.IsValid)
      {
        ModelState.AddModelError("", "");
        return View(model);
      }


      //if (coupon.DateEnd >= coupon.DateStart)
      {
        //coupon.DateStart = coupon.DateStart == DateTime.MinValue ? DateTime.Now : coupon.DateStart;
        //coupon.DateEnd = coupon.DateEnd == DateTime.MinValue ? DateTime.Now : coupon.DateEnd;

        coupon = PromotionViewModel.SaveCoupons(model);

        if (Request.Files.Count > 0)
        {
          try
          {
            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };
            if (couponFiles != null)
            {
              if (couponFiles.InputStream.Length >= 0)
              {
                if (couponFiles.ContentLength > 4194304)
                {
                  ModelState.AddModelError("Couponfile", "The size of the file should not exceed 4 MB");
                  return View(model);
                }

                var fileExt = System.IO.Path.GetExtension(couponFiles.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                  ModelState.AddModelError("Couponfile", "Invalid file type, only the following types (jpg, jpeg,png, gif) are supported.");
                  return View(model);
                }

                string couponFileName = coupon.CouponId + "-" + TextHelper.GenerateRandomText(10) + ".png";
                var source = new Bitmap(couponFiles.InputStream);
                var origin = ImageHelper.GetScaledPicture(source, 210, 156);
                var serverPath = Server.MapPath("~/Userfiles/Promotions/Coupons/" + couponFileName);
                origin.Save(serverPath);
                coupon.CouponImageName = couponFileName;
              }
            }

            if (barcodeFiles != null)
            {
              if (barcodeFiles.InputStream.Length != 0)
              {
                if (barcodeFiles.ContentLength > 4194304)
                {
                  ModelState.AddModelError("file", "The size of the file should not exceed 4 MB");
                  return View(model);
                }

                var barcodefileExt = System.IO.Path.GetExtension(barcodeFiles.FileName).Substring(1);
                if (!supportedTypes.Contains(barcodefileExt.ToLower()))
                {
                  ModelState.AddModelError("Barcodefile", "Invalid file type, only the following types (jpg, jpeg,png, gif) are supported.");
                  return View(model);
                }

                string barcodeFileName = coupon.CouponId + "-" + TextHelper.GenerateRandomText(10) + ".png";
                var sourceBarcode = new Bitmap(barcodeFiles.InputStream);
                var originBarcode = ImageHelper.GetScaledPicture(sourceBarcode, 210, 156);
                var serverPaths = Server.MapPath("~/Userfiles/BarCodes/" + barcodeFileName);
                originBarcode.Save(serverPaths);
                coupon.BarCodeImageName = barcodeFileName;
              }
            }
          }
          catch (Exception ex)
          {
            //TODO : Log exceptino
          }

        }

        PromotionViewModel.SaveCouponImages(coupon);

        if (coupon.QRCode != null)
        {
          string address = "http://chart.apis.google.com/chart?cht=qr&chs=145x145&chl='" + coupon.QRCode + "'&chld=H|0";
          string filePaths = Server.MapPath("~/UserFiles/Promotions/Coupons/QRCodes/" + "QR_" + coupon.CouponId + ".png");
          using (var client = new WebClient())
          using (var stream = client.OpenRead(address))
          using (var file = System.IO.File.Create(filePaths))
          {
            var buffer = new byte[4096];
            int bytesReceived;
            while ((bytesReceived = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
              file.Write(buffer, 0, bytesReceived);
            }
          }
        }
                
        TempData["Notification"] = "Coupon saved successfully!";


        if (Request.Form["continue"] != null && model.CouponId == 0)
        {
          return RedirectToAction("PromotionCouponsEdit", "Merchant", new { promotionid = model.PromotionId });

        }


        return RedirectToAction("PromotionCouponList", "Merchant", new { promotionid = model.PromotionId });

      }
      //else
      //{
      //  ModelState.AddModelError("", "End date should be greater than start date.");
      //  return View(model);
      //}        
    }

    private void PopulateCountryDropDownList(object selectedTeams = null)
    {
      var q = from x in db.MasterCountries
              orderby x.CountryName
              select x;
      ViewBag.Countries = new SelectList(q, "CountryId", "CountryName", selectedTeams);
    }

    private void PopulatePromotionDropDownList(object selectedTeams = null)
    {
      var q = PromotionViewModel.GetByUserName(User.Identity.Name);
      ViewBag.Promotions = new SelectList(q, "PromotionId", "PromotionName", selectedTeams);
    }

    private void PopulateApprovedPromotion(object selectedTeams = null)
    {
      var q = PromotionViewModel.GetByUserNameApproved(User.Identity.Name);
      ViewBag.Promotions = new SelectList(q, "PromotionId", "PromotionName", selectedTeams);
    }

    private void PopulateStateDropDownList(object selectedTeams = null)
    {
      var q = from x in db.MasterStates
              orderby x.StateName
              select x;
      ViewBag.States = new SelectList(q, "StateId", "StateName", selectedTeams);
    }

  }
}
