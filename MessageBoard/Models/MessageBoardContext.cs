using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Models
{
  public class MessageBoardContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Post> Posts { get; set; }

    public DbSet<Topic> Topics { get; set; }

    public DbSet<PostTopic> PostTopics { get; set; }
    public MessageBoardContext(DbContextOptions options) : base(options) { }
  }
}
