using System;

namespace MobileReward.Models
{
  public class CouponDetail
  {
    public int CouponId { get; set; }
    public int PromotionId { get; set; }
    public string PromotionName { get; set; }
    public string CouponName { get; set; }
    public string CouponTitle { get; set; }
    public string QRCode { get; set; }
    public byte[] QRCodeImage { get; set; }
    public string BarCodeImageName { get; set; }
    public byte[] BarCodeImage { get; set; }
    public string CouponImageName { get; set; }
    public byte[] CouponImage { get; set; }
    public int RedemptionLimit { get; set; }
    public int Redeemed { get; set; }
    public string CouponDescription { get; set; }
    public string TermsAndCondition { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public bool Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int UpdateBy { get; set; }
    public DateTime UpdateOn { get; set; }
  }
}