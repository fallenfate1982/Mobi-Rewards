using System;
using System.Collections.Generic;

namespace MobileReward.Models
{
  public class PromotionPartial
  {
    public int PromotionId { get; set; }
    public string PromotionTitle { get; set; }
    public string PromotionName { get; set; }
    public string CountryName { get; set; }
    public bool IsGlobal { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsPaid { get; set; }
    public bool IsApproved { get; set; }
    public bool IsStop { get; set; }
    public int CouponCounts { get; set; }

    public List<CouponPartial> Coupons { get; set; }
  }

  public class CouponPartial
  {
    public int CouponId { get; set; }
    public string CouponName { get; set; }
    public string CouponTitle { get; set; }
    public string QRCode { get; set; }
    public int RedemptionLimit { get; set; }
    public int Redeemed { get; set; }
    public string CouponDescription { get; set; }
    public bool Status { get; set; }
    public string CouponImageName { get; set; }
  }
}