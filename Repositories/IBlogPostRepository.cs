using asp_net_core_mvc.Models.Domain;

namespace asp_net_core_mvc.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetAsync(Guid id);
        Task<BlogPost> AddAsync(BlogPost post);
        Task<BlogPost> UpdateAsync(BlogPost post);
        Task<BlogPost> DeleteAsync(Guid id);
    }
}
