using Microsoft.AspNetCore.Mvc;
using MessageBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MessageBoard.Models;

[Authorize]
public class TopicsController : Controller
{
  private readonly MessageBoardContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public TopicsController(UserManager<ApplicationUser> userManager, MessageBoardContext db)
  {
    _userManager = userManager;
    _db = db;
  }

    public async Task<ApplicationUser> GetCurrentUser()
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    return currentUser;
  }

  public Topic GetTopicById(int id)
  {
    return _db.Topics
    .Include(t => t.PostTopics)
    .ThenInclude(pt => pt.Post)
    .ThenInclude(p => p.User)
    .FirstOrDefault(t => t.TopicId == id);
  }

  [AllowAnonymous]
  public ActionResult Index()
  {
    List<Topic> topics = _db.Topics
    .Include(t => t.PostTopics).ToList();
    List<TopicViewModel> topicViewModels = new(){};
    foreach (Topic t in topics)
    {
      topicViewModels.Add(new TopicViewModel(t));
    }
    return View(topicViewModels);
  }

  [AllowAnonymous]

  public async Task<ActionResult> Details(int id)
  {
    Topic topic = GetTopicById(id);
    List<Post> posts = topic.PostTopics.Select(pt => pt.Post).ToList();
    List<PostViewModel> postViews = new (){};
    ApplicationUser currentUser = await GetCurrentUser();
    foreach(Post p in posts)
    {
      if(p.User == currentUser)
      {
        postViews.Add(new PostViewModel(p, true));
      }
      else
      {
        postViews.Add(new PostViewModel(p));
      }
    }
    return View(new TopicViewModel{
      PostViewModels = postViews,
      Title = topic.Title,
      TopicId = topic.TopicId,
    });
  }

  public ActionResult Delete(int id)
  {
    _db.Remove(GetTopicById(id));
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

}


