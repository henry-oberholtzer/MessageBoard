using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MessageBoard.Models;

namespace MessageBoard.ViewModels;

public class PostForumViewModel
{
  [Range(0, int.MaxValue)]
  public int PostId { get; set; }

  [MaxLength(255)]
  [Display(Name = "Post")]
  [Required]
  public string PostBody { get; set; }

  public SelectList TopicOptions { get; set; }

  [Display(Name = "Assign to topics")]
  public List<int> SelectedTopics { get; set; }

  [Display(Name = "Assign a new topic")]
  public string NewTopic { get; set; }

  public bool Edit { get; set; } = false;

  public Post ToNewPost(ApplicationUser user)
  {
    return new Post(){
      Body = PostBody,
      DatePosted = DateTime.Now,
      DateEdited = DateTime.Now,
      User = user,
    };
  }

  public Post EditPost(Post post)
  {
    post.Body = PostBody;
    post.DateEdited = DateTime.Now;
    return post;
  }

  public PostForumViewModel()
  {

  }

  public PostForumViewModel(Post post, SelectList topics, List<int> selectedTopics, bool edit)
  {
    PostBody = post.Body;
    PostId = post.PostId;
    TopicOptions = topics;
    Edit = edit;
    SelectedTopics = selectedTopics;
  }

}
