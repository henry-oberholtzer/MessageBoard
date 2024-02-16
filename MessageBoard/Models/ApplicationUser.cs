using Microsoft.AspNetCore.Identity;

namespace MessageBoard.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string ProfilePicURL { get; set; }
  }
}
