//using System;
//using System.Collections.Generic;
//using System.Web.Security;
//using System.Web.Services;
//using System.Linq;
//using Postal;
//using WebMatrix.WebData;
//using MobileReward.Models;

//namespace MobileReward.Service
//{
//    [WebService(Namespace = "http://tempuri.org/")]
//    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//    [System.ComponentModel.ToolboxItem(false)]
//    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
//    [System.Web.Script.Services.ScriptService]
//    public class MobiRewardService : System.Web.Services.WebService
//    {

//        [WebMethod]
//        public bool Login(string userName, string password)
//        {
//          try
//          {
//            return WebSecurity.Login(userName, password);
//          }
//          catch (Exception)
//          {
//            throw;
//          }
//        }

//        [WebMethod]
//        public List<Promotion> GetAllPromotions()
//        {
//          try
//          {
//            var db = new MobiRewardDb();
//            return db.Promotions.Where(x => x.Status == true).ToList();
//          }
//          catch (Exception)
//          {
//            throw;
//          } 
//        }

//      [WebMethod]
//      public List<Promotion> GetPromotionsByKeywords(string keywords)
//      {
//        try
//        {
//          var db = new MobiRewardDb();
//          return db.Promotions.Where(x => x.Status == true && 
//                                      (
//                                        x.PromotionName.Contains(keywords)
//                                        || x.PromotionTitle.Contains(keywords))
//                                      ).ToList();
//        }
//        catch (Exception)
//        {
//          throw;
//        } 
//      }

//      [WebMethod]
//      public List<Promotion> GetPromotionsByEndDate(string endDate)
//      {
//        try
//        {
//          DateTime date;
//          if (DateTime.TryParse(endDate, out date))
//          {
//            var db = new MobiRewardDb();
//            return db.Promotions.Where(x => x.Status  &&  x.DateEnd >= date).ToList();
//          }
//          else
//          {
//            throw new Exception("Date format is invalide");
//          }
//        }
//        catch (Exception)
//        {
//          throw;
//        }
//      }

//      [WebMethod]
//      public List<Promotion> GetPromotionsByCountryName(string countryName)
//      {
//        try
//        {
//          var db = new MobiRewardDb();
//          return db.Promotions.Where(x => x.Status && x.MasterCountry.CountryName.Contains(countryName)).ToList();
//        }
//        catch (Exception)
//        {
//          throw;
//        }
//      }

//      [WebMethod]
//      public bool RedeemCoupon(int promotionId)
//      {
//        try
//        {
//          var db = new MobiRewardDb();
//          var promotion = db.Promotions.Where(x => x.PromotionId == promotionId);
//          if (promotion.Any())
//          {
//            var p = promotion.First();
//            /*if (p.RedemptionLimit == p.Grabbed)
//            {
//              throw new Exception("Coupon redemption limit exceded!");
//            }
//            else
//            {
//              p.Grabbed = p.Grabbed + 1;
//              db.SaveChanges();
//            }*/
//          }
//        }
//        catch (Exception)
//        {
//          throw;
//        }

//        return true;
//      }

//      [WebMethod]
//      public bool RegisterMerachant(string userName, string email, string password, 
//                          string companyRegNo, string address, string conactNo)
//      {
//        try
//        {
//          if (WebSecurity.UserExists(userName))
//            throw new Exception("User name already exist!");

//          var db = new MobiRewardDb();
//          var user = db.UserProfiles.Where(x => x.Email == email);
//          if (user.Any())
//            throw new Exception("Email already exist!");

//          string confirmationToken =
//            WebSecurity.CreateUserAndAccount(userName, password,
//                                             new
//                                               {
//                                                 Email = email,
//                                                 CompanyRegistrationNo = companyRegNo,
//                                                 Address = address,
//                                                 ContactNo = conactNo
//                                               },
//                                             true);

//          //Assign Role as Merchant
//          Roles.AddUsersToRole(new[] {userName}, "Merchant");

//          //Send confirmation email
//          dynamic pEmail = new Email("RegEmail");
//          pEmail.To = email;
//          pEmail.UserName = userName;
//          pEmail.ConfirmationToken = confirmationToken;
//          pEmail.Send();
//        }
//        catch (Exception)
//        {
//          throw;
//        }

//        return true;
//      }
//    }
//}
