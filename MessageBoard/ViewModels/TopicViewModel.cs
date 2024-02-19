using MessageBoard.Models;

namespace MessageBoard.ViewModels;

public class TopicViewModel
{
  public TopicViewModel()
  {

  }
  public TopicViewModel(Topic topic)
  {
    TopicId = topic.TopicId;
    Title = topic.Title;
    PostsCount = topic.PostTopics.Count;
  }
  public List<PostViewModel> PostViewModels { get; set; }
  public int TopicId { get; set; }

  public string Title { get; set; }

  public int PostsCount { get; set; }

}
