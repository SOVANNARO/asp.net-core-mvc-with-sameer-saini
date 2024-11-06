using asp_net_core_mvc.Repositories;
using Microsoft.AspNetCore.Mvc;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository blogPostRepository;

    public BlogsController(IBlogPostRepository blogPostRepository)
    {
        this.blogPostRepository = blogPostRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid id)
    {
        if (id == Guid.Empty)
        {
            return NotFound();
        }

        var blogPost = await blogPostRepository.GetByIdAsync(id);

        if (blogPost == null)
        {
            return NotFound();
        }

        return View(blogPost);
    }
}