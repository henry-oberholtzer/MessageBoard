namespace MessageBoard.Models;

public class PostTopic
{
  public int PostTopicId { get; set; }

  public int PostId { get; set; }

  public int TopicId { get; set; }

  public Post Post { get; }

  public Topic Topic { get; }
}
