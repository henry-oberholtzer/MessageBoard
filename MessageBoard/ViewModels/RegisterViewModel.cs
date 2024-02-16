using System.ComponentModel.DataAnnotations;

namespace MessageBoard.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [MaxLength(16)]
    [Display(Name = "Username")]
    public string UserName { get; set;}

    [Display(Name = "Profile Picture (optional)")]

    public string ProfilePicURL { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", 
    ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set;}
  }
}
