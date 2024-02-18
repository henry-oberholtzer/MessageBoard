namespace MessageBoard.Models;

public class Post
{
  public int PostId { get; set; }
  public string Body { get; set; }
  public DateTime DatePosted { get; set; }

#nullable enable
  public DateTime? DateEdited { get; set; }

#nullable disable

  public List<PostTopic> PostTopics { get; }

  public ApplicationUser User { get; set; }
}
