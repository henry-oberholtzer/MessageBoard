namespace MessageBoard.Models;

public class PostTopic
{
  public int PostTopicId { get; set; }

  public int PostId { get; set; }

  public int TopicId { get; set; }

  Post Post { get; }

  Topic Topic { get; }
}
