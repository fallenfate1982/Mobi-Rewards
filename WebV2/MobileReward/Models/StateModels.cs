using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileReward.Models
{
  [Table("MasterState")]
  public class MasterState
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StateId { get; set; }

    [Required(ErrorMessage = "Please enter state name.")]
    [MaxLength(200)]
    public string StateName { get; set; }

    [Required(ErrorMessage = "Please enter state code.")]
    [MaxLength(20)]
    public string StateCode { get; set; }

    [Required]
    [ForeignKey("MasterCountry")]
    public int CountryId { get; set; }

    // navigation props
    [ForeignKey("CountryId")]
    public virtual MasterCountry MasterCountry { get; set; }
  }
}