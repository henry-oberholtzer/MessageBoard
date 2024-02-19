using MessageBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageBoard.ViewModels;

namespace MessageBoard.Controllers
{
  public class HomeController : Controller
  {
    private readonly MessageBoardContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, MessageBoardContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      List<Post> posts = _db.Posts.Include(p => p.User)
      .Include(p => p.PostTopics)
      .ThenInclude(pt => pt.Topic)
      .OrderByDescending(p => p.DatePosted)
      .Take(3).ToList();
      List<Topic> topics = _db.Topics.Include(t => t.PostTopics)
      .OrderByDescending(t => t.DateCreated)
      .Take(3).ToList();
      List<PostViewModel> postViewModels = new (){};
      List<TopicViewModel> topicViewModels = new (){};
      foreach(Post p in posts)
      {
        postViewModels.Add(new PostViewModel(p));
      }
      foreach(Topic t in topics)
      {
        topicViewModels.Add(new TopicViewModel(t));
      }


      ViewBag.PageTitle = "Home";
      return View(new HomeViewModel(postViewModels, topicViewModels));
    }

  }
}
