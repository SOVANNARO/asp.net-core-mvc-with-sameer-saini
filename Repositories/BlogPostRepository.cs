using asp_net_core_mvc.Data;
using asp_net_core_mvc.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace asp_net_core_mvc.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost post)
        {
            await bloggieDbContext.BlogPosts.AddAsync(post);
            await bloggieDbContext.SaveChangesAsync();
            return post;
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            var post = await bloggieDbContext.BlogPosts.FindAsync(id);
            if (post != null)
            {
                bloggieDbContext.BlogPosts.Remove(post);
                await bloggieDbContext.SaveChangesAsync();
            }
            return post;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.FindAsync(id);
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.FindAsync(id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost post)
        {
            var existingPost = await bloggieDbContext.BlogPosts.FindAsync(post.Id);
            if (existingPost != null)
            {
                //existingPost.Title = post.Title;
                //existingPost.Content = post.Content;
                //existingPost.Tags = post.Tags;
                await bloggieDbContext.SaveChangesAsync();
            }
            return existingPost;
        }
    }
}