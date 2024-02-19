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
public class PostsController : Controller
{
  private readonly MessageBoardContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public PostsController(UserManager<ApplicationUser> userManager, MessageBoardContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  private void ClearUnusedTopics()
  {
    List<Topic> unusedTopics = _db.Topics.Include(t => t.PostTopics).Where(p => p.PostTopics.Count == 0).ToList();
    foreach(Topic t in unusedTopics)
    {
      _db.Topics.Remove(t);
    }
    _db.SaveChanges();
  }
  private async Task<Post> FindPostById(int id)
  {
    return await _db.Posts.Include(p => p.User).Include(p => p.PostTopics).ThenInclude(pt => pt.Topic).FirstOrDefaultAsync(p => p.PostId == id);
  }

  private async Task<ApplicationUser> GetCurrentUser()
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    return currentUser;
  }

  private void CreatePostTopics(string newTopics, int postId)
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
    .OrderByDescending(p => p.DatePosted)
    .ToListAsync();

    List<PostViewModel> postViews = new(){};
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

    return View(postViews);
  }

  public async Task<IActionResult> UserPosts()
  {
    ApplicationUser currentUser = await GetCurrentUser();
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
    ApplicationUser currentUser = await GetCurrentUser();
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
    if(target.User != await GetCurrentUser())
    {
      return RedirectToAction("Details", "Post", new { id = target.PostId });
    }

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
    ClearUnusedTopics();
    return RedirectToAction("Index");
  }
  
  [AllowAnonymous]
  public async Task<IActionResult> Details(int id)
  {
    Post target = await FindPostById(id);
    if(target.User != await GetCurrentUser())
    {
      return View(new PostViewModel(target));
    }
    return View(new PostViewModel(target, true));
  }

  [HttpPost]
  public async Task<ActionResult> Delete(int id)
  {
    Post target = await FindPostById(id);
    if(target.User != await GetCurrentUser())
    {
      return RedirectToAction("Details", "Post", new { id = target.PostId });
    }
    _db.Posts.Remove(target);
    _db.SaveChanges();
    ClearUnusedTopics();
    return RedirectToAction("Index");
  }


}
