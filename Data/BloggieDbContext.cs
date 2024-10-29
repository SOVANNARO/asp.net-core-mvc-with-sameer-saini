using asp_net_core_mvc.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace asp_net_core_mvc.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
