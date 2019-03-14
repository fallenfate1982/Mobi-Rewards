using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileReward.Models
{
  [Table("MasterCountry")]
  public class MasterCountry
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Please enter country name.")]
    [MaxLength(200)]
    public string CountryName { get; set; }

    [Required]
    [MaxLength(10)]
    public string CountryCode { get; set; }

  }
}