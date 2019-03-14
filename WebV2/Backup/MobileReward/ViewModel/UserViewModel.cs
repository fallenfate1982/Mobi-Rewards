using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileReward.ViewModel
{
  public class UserViewModel
  {
    public int UserId { get; set; }

    [Required(ErrorMessage = "Please enter user name")]
    [MaxLength(20)]
    public string UserName { get; set; }
    [MaxLength(50)]
    [Required(ErrorMessage= "Please enter first name")]
    public string FirstName { get; set; }
    [MaxLength(50)]
    [Required(ErrorMessage = "Please enter first name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter email id")]
    [StringLength(30)]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._]+@[\w\.]+\.[\w]{2,3}$", ErrorMessage = "Please Enter your correct Email id")]
    [EmailAddress(ErrorMessage = "Please enter valid email id.")]
    public string Email { get; set; }

    //[Required(ErrorMessage = "Please enter registration no.")]
    [MaxLength(300)]
    public string CompanyRegistrationNo { get; set; }

    [Required(ErrorMessage = "Please enter contact no.")]
    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Entered contact no format is not valid.")]
    public string ContactNo { get; set; }

    [MaxLength(1000)]
    public string Address { get; set; }

    public bool Status { get; set; }

    public int? UpdateBy { get; set; }
    public DateTime? UpdateOn { get; set; }

    public int? ParentId { get; set; }

    public bool? IsUser { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } 

  }

  public class EditViewModel
  {
    public int UserId { get; set; }

    [Required(ErrorMessage = "Please enter user name")]
    [MaxLength(20)]
    public string UserName { get; set; }
    [MaxLength(50)]
    [Required(ErrorMessage = "Please enter first name")]
    public string FirstName { get; set; }
    [MaxLength(50)]
    [Required(ErrorMessage = "Please enter first name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter email id")]
    [StringLength(30)]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._]+@[\w\.]+\.[\w]{2,3}$", ErrorMessage = "Please Enter your correct Email id")]
    [EmailAddress(ErrorMessage = "Please enter valid email id.")]
    public string Email { get; set; }
    public bool Status { get; set; }
  }
}