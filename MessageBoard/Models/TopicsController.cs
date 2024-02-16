using Microsoft.AspNetCore.Mvc;
using MessageBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

  public Topic GetTopicById(int id)
  {
    return _db.Topics
    .Include(t => t.PostTopics)
    .ThenInclude(pt => pt.Post)
    .ThenInclude(p => p.User)
    .FirstOrDefault(t => t.TopicId == id);
  }

  public ActionResult Index()
  {
    List<Topic> topics = _db.Topics
    .Include(t => t.PostTopics).ThenInclude(pt => pt.Post).ToList();
    return View(topics);
  }

  public ActionResult Details(int id)
  {
    return View(GetTopicById(id));
  }

}


