using MessageBoard.Models;

namespace MessageBoard.ViewModels;

public class TopicViewModel
{
  public List<PostViewModel> PostViewModels { get; set; }
  public int TopicId { get; set; }

  public string Title { get; set; }

}
