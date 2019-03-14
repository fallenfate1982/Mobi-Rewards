using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileReward.Models
{
  [Table("Promotions")]
  public class Promotion
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PromotionId { get; set; }

    [Required(ErrorMessage = "Please enter advertisement name.")]
    [MaxLength(200)]
    public string AdvertisementName { get; set; }

    [Required(ErrorMessage = "Please enter promotion name.")]
    [MaxLength(200)]
    public string PromotionName { get; set; }

    [Required(ErrorMessage = "Please enter promotion title.")]
    [MaxLength(200)]
    public string PromotionTitle { get; set; }

    [Required(ErrorMessage = "Please enter description.")]
    [Column(TypeName = "ntext")]
    public string PromotionDescription { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
    [Required(ErrorMessage = "Please enter start date.")]
    [Column(TypeName = "DateTime2")]
    public DateTime DateStart { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
    [Required(ErrorMessage = "Please enter end date.")]
    [Column(TypeName = "DateTime2")]
    public DateTime DateEnd { get; set; }



    /*[Numeric(ErrorMessage = "Please enter numeric only.")]  
    [RegularExpression("^[0-9]+$", ErrorMessage = "Entered redemption value is not valid.")]
    public Int64? RedemptionLimit { get; set; }*/

    //public Int32? Grabbed { get; set; }

    //public string BarCodeNumber { get; set; }

    public string FilePath { get; set; }
    [ForeignKey("MasterCountry")]
    public int? TargetCountryId { get; set; }
    [ForeignKey("CountryId")]
    public virtual MasterCountry MasterCountry { get; set; }

    [ForeignKey("MasterState")]
    public int? TargetStateId { get; set; }
    [ForeignKey("StateId")]
    public virtual MasterState MasterState { get; set; }

    [ForeignKey("UserProfile")]
    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual UserProfile UserProfile { get; set; }

    public bool IsGlobal { get; set; }

    public bool IsApproved { get; set; }
    public bool IsPaid { get; set; }

    public bool? IsStop { get; set; }
    public double? PaymentReceived { get; set; }
    public bool Status { get; set; }
    public int CreatedBy { get; set; }
    [Column(TypeName = "DateTime2")]
    public DateTime CreatedOn { get; set; }
    public int UpdateBy { get; set; }
    [Column(TypeName = "DateTime2")]
    public DateTime UpdateOn { get; set; }

    [NotMapped]
    public bool IsCoupon { get; set; }

    [NotMapped]
    public CouponDetail CoupanDetails { get; set; }

  }
}