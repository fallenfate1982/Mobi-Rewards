using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MobileReward.Models;
using MobileReward.ReportsForms;
using MobileReward.ViewModel;

namespace MobileReward.Controllers
{
  [Authorize(Roles = ("Admin"))]
  public class AdminController : Controller
  {
    MobiRewardDb db = new MobiRewardDb();
    PromotionViewModel PromotionViewModel = new PromotionViewModel();
    const int pageSize = 10;

    public ActionResult AdminPromotionList(int page=1)
    {

      var list = PromotionViewModel.GetAllByPagged(page, pageSize);
      //var promotion = db.Promotions.FirstOrDefault(x => x.Status == true);
      //var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      //ViewData["userName"] = user.FirstName + " " + user.LastName;
      ViewBag.CurrentPage = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = Math.Ceiling((double)PromotionViewModel.GetAll().Count() / pageSize);
      return View(list);
    }

    [HttpPost]
    public ActionResult AdminPromotionList(string keyword, string command,int page=1)
    {
      var list =new  List<Promotion>();
      if (command == "Reset")
      {
        list = PromotionViewModel.GetAllByPagged(page, pageSize);
        ViewBag.TotalPages = Math.Ceiling((double)PromotionViewModel.GetAll().Count() / pageSize);// PromotionViewModel.AdminPromotionSearch(keyword);
      }
      else
      {
        list = PromotionViewModel.AdminPromotionSearch(keyword, page, pageSize);
        ViewBag.TotalPages = Math.Ceiling((double)list.Count() / pageSize);
      
      }
        
        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
      return View(list);
    }

    public ActionResult Dashboard()
    {
      var list = PromotionViewModel.GetIsApproved();
      // var promotion = db.Promotions.FirstOrDefault(x => x.Status == true);

      //  var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      // ViewData["userName"] = user.FirstName + " " + user.LastName;
      return View(list);
    }

    public ActionResult LatestPromotions()
    {
      var list = PromotionViewModel.GetLatest();
      //var promotion = db.Promotions.FirstOrDefault(x => x.Status == true);
      //var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      //ViewData["userName"] = user.FirstName + " " + user.LastName;
      return View(list);
    }

    public ActionResult AdminPromotionEdit(int promotionId = 0)
    {
      var promotion = db.Promotions.FirstOrDefault(x => x.PromotionId == promotionId);
      var user = db.UserProfiles.FirstOrDefault(x => x.UserId == promotion.CreatedBy);
      ViewData["userName"] = user.FirstName + " " + user.LastName;
      return View(promotion);
    }

    [HttpPost]
    public ActionResult AdminPromotionEdit(Promotion model, HttpPostedFileBase photoFile)
    {
      var promotion = new Promotion();
      if (ModelState.IsValid)
      {
        try
        {

          promotion = PromotionViewModel.Save(model);
        }
        catch (Exception ex)
        {
          //TODO : Log exceptino
        }

        //ViewBag.Status = "Profile updated successfully!";
        TempData["Notification"] = "Promotion saved successfully!";

      }
      else
      {
        ModelState.AddModelError("", "Error while updating Promotion.");
      }
      return RedirectToAction("AdminPromotionList", "Admin");
      //return View();
    }

    public ActionResult PromotionDetails(int promotionid)
    {
      var p = PromotionViewModel.GetBy(promotionid);

      return View(p);
    }

    public ActionResult Reports()
    {
      return View();
    }

    public ActionResult Users(int page = 1)
    {

        var userList = db.UserProfiles.Where(x => x.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId).OrderByDescending((x => x.FirstName)).OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
      ViewBag.CurrentPage = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = Math.Ceiling((double)db.UserProfiles.Where(x => x.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId).Count() / pageSize);
      return View(userList);
    }
    //Search User
    [HttpPost]
    public ActionResult Users(string keyword, string command, int page = 1)
    {
      var list = new List<UserProfile>();

      if (command == "Reset")
      {
        list = db.UserProfiles.Where(x => x.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId).OrderByDescending((x => x.FirstName)).Skip((page - 1) * pageSize).Take(pageSize).OrderBy(x=>x.UserName).ToList();
      }
      else
      {
          list = db.UserProfiles.Where(x => x.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId && (x.UserName.Contains(keyword) || x.FirstName.Contains(keyword) || x.LastName.Contains(keyword))).OrderBy(x => x.UserName).OrderByDescending((x => x.FirstName)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
      }

      ViewBag.CurrentPage = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = Math.Ceiling((double)list.Count() / pageSize);
      return View(list);
    }


    public ActionResult DelUsers(int id)
    {
      var user = db.UserProfiles.First(x => x.UserId == id);
      if (user != null)
      {
        user.Status = false;
        UpdateModel(user);
        db.SaveChanges();
        return RedirectToAction("Users");
      }
      else
      {
        ModelState.AddModelError("", "User does not exist");
        return View();
      }
    }

    public ActionResult ActivateUsers(int id)
    {
      var user = db.UserProfiles.First(x => x.UserId == id);
      if (user != null)
      {
        user.Status = true;
        UpdateModel(user);
        db.SaveChanges();
        return RedirectToAction("Users");
      }
      else
      {
        ModelState.AddModelError("", "User does not exist");
        return View();
      }
    }

    public ActionResult StandardReport()
    {
      ViewBag.Promotions = new SelectList(db.Promotions.ToList(), "PromotionId", "PromotionName");
      var userName = db.UserProfiles.ToList();

      var list = (from x in db.UserProfiles.ToList()
                 where Roles.IsUserInRole(x.UserName, "Merchant")
                 select new MerchantProfile
                 {
                   MerchantId = x.UserId,
                   MerchantName = x.UserName //x.FirstName + " " + x.LastName
                 }).ToList().OrderBy(x=>x.MerchantName);

      ViewBag.Merchants = new SelectList(list, "MerchantId", "MerchantName");

      return View(new StandardReportModel());
    }

    [HttpPost]
    public string StandardReport(string userId, string promotionId, bool isPaid, bool isApproved, bool isStop, DateTime? StartDate, DateTime? EndDate)
    {
      {
        string res = "0";
        List<StandardReportModel> list = new List<StandardReportModel>();
        var promotionModel = db.Promotions.ToList();
        if (promotionModel != null && promotionModel.Count > 0)
        {
          foreach (var item in promotionModel)
          {
            var coupons = db.PromotionsCoupon.Where(x => x.PromotionId == item.PromotionId).ToList();
            foreach (var c in coupons)
            {
              var lstDetails = new StandardReportModel();
              lstDetails.PromotionId = Convert.ToInt32(item.PromotionId);
              lstDetails.PromotionName = item.PromotionName;
              lstDetails.PromotionTitle = item.PromotionTitle;
              lstDetails.CouponName = c.CouponName;
              lstDetails.CouponTitle = c.CouponTitle;
              lstDetails.CouponId = c.CouponId;
              lstDetails.Redeemed = c.Redeemed;
              lstDetails.RedemptionLimit = c.RedemptionLimit;
              lstDetails.MerchantName = item.UserProfile.UserName;
              lstDetails.UserId = item.CreatedBy;
              lstDetails.IsApproved = item.IsApproved;
              lstDetails.IsPaid = item.IsPaid;
              lstDetails.DateStart = item.DateStart;
              lstDetails.DateEnd = item.DateEnd;
              lstDetails.PaymentReceived = item.PaymentReceived ?? 0;
              if (item.IsStop == true)
                lstDetails.IsStop = true;
              else
                lstDetails.IsStop = false;
              list.Add(lstDetails);
            }
          }
        }


        if (userId != "---Select Merchant---" && userId != "")
        {
          int id = Convert.ToInt32(userId);
          if (id != 0)
          {
            list = list.Where(x => x.UserId == id).ToList();
          }
        }

        if (promotionId != "---Select Promotion---" && promotionId != "")
        {
          int pId = Convert.ToInt32(promotionId);
          if (pId != 0)
          {
            list = list.Where(x => x.PromotionId == pId).ToList();
          }
        }

        if (isPaid == true)
        {
          list = list.Where(x => x.IsPaid == true).ToList();
        }

        if (isApproved == true)
        {
          list = list.Where(x => x.IsApproved == true).ToList();
        }

        if (isStop == true)
        {
          list = list.Where(x => x.IsStop == true).ToList();
        }

        if (EndDate != null && StartDate != null)
        {
          list = list.Where(x => (x.DateStart >= StartDate && x.DateStart <= EndDate) || (x.DateEnd >= StartDate && x.DateEnd <= EndDate)).ToList();
        }

        if (list.Count > 0)
        {
          this.HttpContext.Session["ReportName"] = "StandardReport.rpt";
          this.HttpContext.Session["rptSource"] = list;
          res = "1";
        }
        return res;
      }
    }


    [HttpPost]
    public ActionResult GetPromotions(string MerchantId)
    {
      var pid = new List<Promotion>();
      int id = 0;
      if (MerchantId == "")
      {
        pid = db.Promotions.Where(x => x.Status == true).ToList();
      }
      else
      {
        id = Convert.ToInt32(MerchantId);
        pid = db.Promotions.Where(x => x.CreatedBy == id && x.Status == true).ToList();
      }
      var versions = from p in db.Promotions
                     where p.CreatedBy == id
                     select new PromotionPartial
                     {
                       PromotionId = p.PromotionId,
                       PromotionName = p.PromotionName
                     };

      if (pid == null)
        return Json(null);

      return Json(pid);
    }

  }
}


