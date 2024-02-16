namespace MessageBoard.Models;

public class Topic
{
  public int TopicId { get; set; }

  public string Title { get; set; }

  public DateTime DateCreated { get; set; }

  public List<PostTopic> PostTopics { get; set; }
}
