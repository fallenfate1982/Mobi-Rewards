using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileReward.Models
{
  [Table("PromotionsCoupon")]
  public class PromotionsCoupon
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CouponId { get; set; }

    [ForeignKey("Promotions")]
    [Required(ErrorMessage = "Please enter promotion name.")]
    public int PromotionId { get; set; }
    [ForeignKey("PromotionId")]
    public virtual Promotion Promotions { get; set; }

    [Required(ErrorMessage = "Please enter coupon name.")]
    [MaxLength(200)]
    public string CouponName { get; set; }

    [Required(ErrorMessage = "Please enter coupon title.")]
    [MaxLength(500)]
    public string CouponTitle { get; set; }

    //[Required(ErrorMessage = "Please add QR Code.")]
    [MaxLength(300)]
    public string QRCode { get; set; }

    [MaxLength(100)]
    public string BarCodeImageName { get; set; }

    [MaxLength(100)]
    public string CouponImageName { get; set; }

    [Required(ErrorMessage = "Please enter redemption limit.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
    public int RedemptionLimit { get; set; }

    public int Redeemed { get; set; }

    [Required(ErrorMessage = "Please enter description.")]
    [Column(TypeName = "ntext")]
    public string CouponDescription { get; set; }

    [Required(ErrorMessage = "Please enter term & Condition.")]
    [Column(TypeName = "ntext")]
    public string TermsAndCondition { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
    [Required(ErrorMessage = "Please enter start date.")]
    [Column(TypeName = "DateTime2")]
    public DateTime DateStart { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
    [Required(ErrorMessage = "Please enter end date.")]
    [Column(TypeName = "DateTime2")]
    public DateTime DateEnd { get; set; }

    public bool Status { get; set; }

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int UpdateBy { get; set; }
    public DateTime UpdateOn { get; set; }
  }
}