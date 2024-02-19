using MessageBoard.Models;

namespace MessageBoard.ViewModels;

public class PostViewModel
{
  public PostViewModel(Post post, bool editable = false)
  {
    Editable = editable;
    PostId = post.PostId;
    ProfilePicURL = post.User.ProfilePicURL;
    UserName = post.User.UserName;
    Body = post.Body;
    DatePosted = post.DatePosted;
    DateEdited = post.DateEdited;
    Topics = post.PostTopics.Select(pt => pt.Topic).ToList();
  }

  public bool Editable { get; set; }

  public int PostId { get; set; }

  public string ProfilePicURL { get; set; }
  
  public string UserName { get; set; }

  public string Body { get; set; }

  public DateTime DatePosted { get; set; }

  public List<Topic> Topics { get; set; }

#nullable enable
  public DateTime? DateEdited { get; set; }

#nullable disable


}
