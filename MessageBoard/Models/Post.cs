namespace MessageBoard.Models;

public class Post
{
  public int PostId { get; set; }
  public string Body { get; set; }
  public DateTime DatePosted { get; set; }

  public DateTime DateEdited { get; set; }

  public List<PostTopic> PostTopics { get; }
}
