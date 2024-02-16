using System.Security.Claims;
using MessageBoard.Models;
using MessageBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Controllers;

[Authorize]
public class PostsController: Controller
{
  private readonly MessageBoardContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public PostsController(UserManager<ApplicationUser> userManager, MessageBoardContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public Post FindPostById(int id)
  {
    return _db.Posts.Include(p => p.PostTopics).ThenInclude(pt => pt.Topic).Include(p => p.User).FirstOrDefault(p => p.PostId == id);
  }

  public async Task<ActionResult> Index()
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    List<Post> userPosts = _db.Posts
    .Include(p => p.PostTopics)
    .Where(entry => entry.User.Id == currentUser.Id)
    .ToList();
    return View(userPosts);
  }

  public ActionResult Create()
  {
    SelectList topics = new(_db.Topics.ToList(), "TopicId", "Title");
    return View("PostForm", new PostForumViewModel{ TopicOptions = topics});
  }

  [HttpPost]
  public async Task<ActionResult> Create(PostForumViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View("PostForm", model);
    }
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    Post newPost = model.ToNewPost(currentUser);
    _db.Posts.Add(newPost);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult Edit(int id)
  {
    Post target = FindPostById(id);
    SelectList topics = new(_db.Topics.ToList(), "TopicId", "Title");
    List<int> selectedTopics = target.PostTopics.Select(pt => pt.TopicId).ToList();
    return View("PostForm", new PostForumViewModel(target, topics, selectedTopics, true));
  }

  [HttpPost]
  public ActionResult Edit(PostForumViewModel model)
  {
    if(!ModelState.IsValid)
    {
      return View("PostForm", model);
    }
    _db.Posts.Update(model.EditPost(FindPostById(model.PostId)));
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult Details(int id)
  {
    return View(FindPostById(id));
  }

  [HttpPost]
  public ActionResult Delete(int id)
  {
    _db.Posts.Remove(FindPostById(id));
    _db.SaveChanges();
    return RedirectToAction("Index");
  }


}