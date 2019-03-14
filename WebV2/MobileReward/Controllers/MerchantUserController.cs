using MobileReward.Models;
using MobileReward.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;


namespace MobileReward.Controllers
{
    //[Authorize(Roles = ("Merchant"))]
    public class MerchantUserController : Controller
    {
      private MobiRewardDb db = new MobiRewardDb();
      const int pageSize = 10;
      //
      // GET: /MerchantUser/

      public ActionResult Index()
      {
        return View();
      }

      [HttpGet]
      public ActionResult List(int page = 1)
      {
        var userList = new List<UserViewModel>();
        var userCurrent = db.UserProfiles.FirstOrDefault(x => x.UserName == User.Identity.Name);
        var userProfile = db.UserProfiles.Where(x => x.ParentId == userCurrent.UserId).OrderBy(x => x.UserId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        foreach (var item in userProfile)
        {
          var user = new UserViewModel();
          user.UserId = item.UserId;
          user.UserName = item.UserName;
          user.FirstName = item.FirstName;
          user.LastName = item.LastName;
          user.Email = item.Email;
          user.ContactNo = item.ContactNo;
          user.Status = item.Status;
          userList.Add(user);
        }
        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = Math.Ceiling((double)userProfile.Count() / pageSize);
        return View(userList.ToList());
      }
      [HttpPost]
      public ActionResult List(string keyword, string command, int page = 1)
      {
        var list = new List<UserViewModel>();
        if (command == "Reset")
        {
          list = Reset(page, pageSize);
          ViewBag.TotalPages = Math.Ceiling((double)list.Count() / pageSize);
        }
        else
        {
          list = Search(keyword, page, pageSize);
          ViewBag.TotalPages = Math.Ceiling((double)list.Count() / pageSize);
        }

        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        return View(list);
      }

      [HttpGet]
      public ActionResult Create()
      {
        var user = new UserViewModel();
        return View(user);
      }
      [HttpPost]
      public ActionResult Create(UserViewModel model)
      {
        if (ModelState.IsValid)
        {
          try
          {
            //Check if user already exist 
            if (WebSecurity.UserExists(model.UserName))
            {
              TempData["ErrorNotification"] = "User name already exist!";
              return View(model);
            }

            var user = db.UserProfiles.Where(x => x.Email == model.Email);
            if (user.Any())
            {
              TempData["ErrorNotification"] = "Email already exist!";
              return View(model);
            }
            var profile = db.UserProfiles.FirstOrDefault(x => x.UserName == User.Identity.Name);
            string confirmationToken =
                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                        new
                        {
                          FirstName = model.FirstName,
                          LastName = model.LastName,
                          Email = model.Email,
                          ContactNo = model.ContactNo,
                          Address = profile.Address,
                          ParentId = profile.UserId,
                          CompanyRegistrationNo = profile.CompanyRegistrationNo,
                          Status = true,
                          IsUser = true
                        },
                        true);

            //Assign Role as user
            Roles.AddUsersToRole(new[] { model.UserName }, "User");
            WebSecurity.ConfirmAccount(confirmationToken);
            TempData["Notification"] = "Profile Save successfully!";
            return RedirectToAction("List", "MerchantUser");
          }
          catch (MembershipCreateUserException e)
          {
            TempData["ErrorNotification"] = "Error writing to Create.";
          }
        }
        return View(model);
      }

      [HttpGet]
      public ActionResult Edit(int id)
      {
        var profile = db.UserProfiles.FirstOrDefault(x => x.UserId == id);
        var model = new EditViewModel
        {
          UserId = profile.UserId,
          UserName = profile.UserName,
          FirstName = profile.FirstName,
          LastName = profile.LastName,
          Email = profile.Email,
          Status = profile.Status
        };
        return View(model);
      }

      [HttpPost]
      public ActionResult Edit(EditViewModel model)
      {
        if (ModelState.IsValid)
        {
          try
          {
            var profile = db.UserProfiles.FirstOrDefault(x => x.UserId == model.UserId);
            if (profile.UserName != model.UserName)
            {
              TempData["ErrorNotification"] = "Username already exist!";
              return View(model);
            }
            if (profile.Email != model.Email)
            {
              TempData["ErrorNotification"] = "Email already exist!";
              return View(model);
            }
            if (profile != null)
            {
              profile.UserName = model.UserName;
              profile.FirstName = model.FirstName;
              profile.LastName = model.LastName;
              profile.Email = model.Email;
              profile.Status = model.Status;

              db.SaveChanges();
              TempData["Notification"] = "Profile updated successfully!";
              return RedirectToAction("List");
            }
          }
          catch
          {
            TempData["ErrorNotification"] = "Error writing to Edit.";
          }
        }
        return View();
      }

      public List<UserViewModel> Search(string keyword, int pageNo, int pageSize, string userName = "", bool isActiveOnly = false)
      {
        var userList = new List<UserViewModel>();
        var q = db.UserProfiles.Where(x => x.UserName.Contains(keyword) || x.FirstName.Contains(keyword) ||
          x.LastName.Contains(keyword));

        var userCurrent = db.UserProfiles.FirstOrDefault(x => x.UserName == User.Identity.Name);
        if (!String.IsNullOrWhiteSpace(userName))
          q = q.Where(x => x.UserName == userName);
        
        foreach (var item in q.Where(x => x.ParentId == userCurrent.UserId))
        {
          var user = new UserViewModel();
          user.UserId = item.UserId;
          user.UserName = item.UserName;
          user.FirstName = item.FirstName;
          user.LastName = item.LastName;
          user.Email = item.Email;
          user.Status = item.Status;
          userList.Add(user);
        }
        return userList.OrderBy(x => x.UserName).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
      }

      public List<UserViewModel> Reset(int pageNo, int pageSize, bool isActiveOnly = false)
      {
        var userList = new List<UserViewModel>();
        var userCurrent = db.UserProfiles.FirstOrDefault(x => x.UserName == User.Identity.Name);
        var q = db.UserProfiles.Where(x => x.ParentId == userCurrent.UserId).OrderBy(x => x.UserName).Take(pageSize);
        foreach (var item in q)
        {
          var user = new UserViewModel();
          user.UserId = item.UserId;
          user.UserName = item.UserName;
          user.FirstName = item.FirstName;
          user.LastName = item.LastName;
          user.Email = item.Email;
          user.Status = item.Status;
          userList.Add(user);
        }
        return userList.ToList();
      }

    }
}
