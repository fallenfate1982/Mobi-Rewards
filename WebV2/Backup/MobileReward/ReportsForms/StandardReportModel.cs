using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileReward.ReportsForms
{
  public class StandardReportModel
  {
    public int PromotionId { get; set; }
    public string AdvertisementName { get; set; }
    public string PromotionName { get; set; }
    public string PromotionTitle { get; set; }
    public string PromotionDescription { get; set; }
    public int CouponId { get; set; }
    public string CouponName { get; set; }
    public string CouponTitle { get; set; }
    public string QRCode { get; set; }
    public string CouponDescription { get; set; }
    public int Redeemed { get; set; }
    public int RedemptionLimit { get; set; }
    public string MerchantName { get; set; }
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public bool IsApproved { get; set; }
    public bool IsStop { get; set; }
    public double PaymentReceived { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
  }
}
