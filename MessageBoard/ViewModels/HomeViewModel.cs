namespace MessageBoard.ViewModels;

public class HomeViewModel
{
  public HomeViewModel(List<PostViewModel> postViewModels, List<TopicViewModel> topicViewModels)
  {
    PostViewModels = postViewModels;
    TopicViewModels = topicViewModels;
  }
  public List<PostViewModel> PostViewModels { get; set; }

  public List<TopicViewModel> TopicViewModels { get; set; }

}
