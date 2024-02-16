using System.Security.Claims;
using MessageBoard.Models;
using MessageBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Controllers;

[Authorize]
public class PostsController : Controller
{
  private readonly MessageBoardContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public PostsController(UserManager<ApplicationUser> userManager, MessageBoardContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public async Task<Post> FindPostById(int id)
  {
    return await _db.Posts.Include(p => p.User).Include(p => p.PostTopics).ThenInclude(pt => pt.Topic).FirstOrDefaultAsync(p => p.PostId == id);
  }

  public void CreatePostTopics(string newTopics, int postId)
  {
    if (!string.IsNullOrEmpty(newTopics))
    {
      List<string> newTitles = newTopics.Split(",").ToList();
      foreach (string title in newTitles)
      {
        string normalized = title.Trim().Normalize();
        if (!_db.Topics.Any(t => t.Title.ToUpper() == normalized.ToUpper()))
        {
          Topic topic = new()
          {
            Title = normalized,
            DateCreated = DateTime.Now,
          };
          _db.Topics.Add(topic);
          _db.SaveChanges();
          _db.PostTopics.Add(new PostTopic{
            PostId =  postId,
            TopicId = topic.TopicId
          });
        }
      }
      _db.SaveChanges();
    }
  }
  [AllowAnonymous]
  public async Task<ActionResult> Index()
  {
    List<Post> posts = await _db.Posts
    .Include(p => p.User)
    .Include(p => p.PostTopics)
    .ThenInclude(pt => pt.Topic)
    .OrderBy(p => p.DatePosted)
    .ToListAsync();
    return View(posts);
  }

  public async Task<IActionResult> UserPosts()
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    List<Post> userPosts = _db.Posts
    .Include(p => p.PostTopics)
    .Where(entry => entry.User.Id == currentUser.Id)
    .ToList();
    return PartialView(userPosts);
  }

  public ActionResult Create()
  {
    SelectList topics = new(_db.Topics.ToList(), "TopicId", "Title");
    return View("PostForm", new PostForumViewModel { TopicOptions = topics });
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
    CreatePostTopics(model.NewTopic, newPost.PostId);  
    return RedirectToAction("Index");
  }

  public async Task<ActionResult> Edit(int id)
  {
    Post target = await FindPostById(id);
    SelectList topics = new(_db.Topics.ToList(), "TopicId", "Title");
    List<int> selectedTopics = target.PostTopics.Select(pt => pt.TopicId).ToList();
    return View("PostForm", new PostForumViewModel(target, topics, selectedTopics, true));
  }

  [HttpPost]
  public async Task<ActionResult> Edit(PostForumViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View("PostForm", model);
    }
    Post target = await FindPostById(model.PostId);
    foreach (PostTopic pt in target.PostTopics)
    {
      _db.PostTopics.Remove(pt);
    }
    _db.SaveChanges();
    foreach (int i in model.SelectedTopics) {
      _db.PostTopics.Add(new PostTopic{
        PostId = model.PostId,
        TopicId = i
      });
    }
    CreatePostTopics(model.NewTopic, model.PostId);
    _db.Posts.Update(model.EditPost(target));
    _db.SaveChanges();
    return RedirectToAction("Index");
  }
  
  [AllowAnonymous]
  public async Task<IActionResult> Details(int id)
  {
    return View(await FindPostById(id));
  }

  [HttpPost]
  public async Task<ActionResult> Delete(int id)
  {
    _db.Posts.Remove(await FindPostById(id));
    _db.SaveChanges();
    return RedirectToAction("Index");
  }


}
