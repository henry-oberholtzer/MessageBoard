using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MessageBoard.Models;
using System.Threading.Tasks;
using MessageBoard.ViewModels;

namespace MessageBoard.Controllers;

public class AccountController : Controller
{
  private readonly MessageBoardContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;

  public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, MessageBoardContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _db = db;
  }

  public ActionResult Index()
  {
    return View();
  }

  public IActionResult Register()
  {
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> Register (RegisterViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }
    else
    {
      ApplicationUser user = new() { UserName = model.UserName, Email = model.Email, ProfilePicURL = model.ProfilePicURL };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        foreach (IdentityError error in result.Errors)
        {
          ModelState.AddModelError("", error.Description);
        }
        return View(model);
      }
    }
  }
}
