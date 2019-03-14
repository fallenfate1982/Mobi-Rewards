using System;
using System.ComponentModel.DataAnnotations;

namespace MobileReward.Models
{
  public class RegisterExternalLoginModel
  {
    [Required]
    [Display(Name = "User name")]
    
    public string UserName { get; set; }

    public string ExternalLoginData { get; set; }
  }

  public class LocalPasswordModel
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }

  public class LocalResetPasswordModel
  {
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }

  public class LoginModel
  {
    [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
  }

  public class RegisterModel
  {
    [Required]
    [RegularExpression(@"^[\S]*$", ErrorMessage = "Space is not allowed in user name")]
    [Display(Name = "User name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please enter email id.")]
    [StringLength(30)]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Please enter valid email id.")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    //[Required(ErrorMessage = "Please enter registration no.")]
    [MaxLength(300)]
    public string CompanyRegistrationNo { get; set; }

    [Required(ErrorMessage = "Please enter address.")]
    [MaxLength(1000)]
    public string Address { get; set; }

    [Required]
    //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
    //[RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessage = "Entered contact number format is not valid.")]

    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Entered contact no format is not valid.")]
    public string ContactNo { get; set; }

    public bool? Status { get; set; }
  }
  
  public class ExternalLogin
  {
    public string Provider { get; set; }
    public string ProviderDisplayName { get; set; }
    public string ProviderUserId { get; set; }
  }
}
