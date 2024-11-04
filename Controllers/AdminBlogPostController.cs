using asp_net_core_mvc.Models.Domain;
using asp_net_core_mvc.Models.ViewModels;
using asp_net_core_mvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_net_core_mvc.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AdminBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdminBlogPostRequest adminBlogPostRequest)
        {
            // Convert PublishedDate to UTC
            var publishedDateUtc = DateTime.SpecifyKind(adminBlogPostRequest.PublishedDate, DateTimeKind.Utc);

            // Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = adminBlogPostRequest.Heading,
                PageTitle = adminBlogPostRequest.PageTitle,
                Content = adminBlogPostRequest.Content,
                ShortDescription = adminBlogPostRequest.ShortDescription,
                FeaturedImageUrl = adminBlogPostRequest.FeaturedImageUrl,
                UrlHandle = adminBlogPostRequest.UrlHandle,
                PublishedDate = publishedDateUtc, // Use the UTC date
                Author = adminBlogPostRequest.Author,
                Visiable = adminBlogPostRequest.Visiable,
            };

            // Map Tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in adminBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;
            await blogPostRepository.AddAsync(blogPost);
            // Redirect to the List action after adding the blog post
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var tags = await tagRepository.GetAllAsync();

            var model = new AdminBlogPostRequest
            {
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Visiable = blogPost.Visiable,
                SelectedTags = blogPost.Tags?.Select(t => t.Id.ToString()).ToArray() ?? new string[0], // Add null check
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AdminBlogPostRequest adminBlogPostRequest)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Update the blog post properties
            blogPost.Heading = adminBlogPostRequest.Heading;
            blogPost.PageTitle = adminBlogPostRequest.PageTitle;
            blogPost.Content = adminBlogPostRequest.Content;
            blogPost.ShortDescription = adminBlogPostRequest.ShortDescription;
            blogPost.FeaturedImageUrl = adminBlogPostRequest.FeaturedImageUrl;
            blogPost.UrlHandle = adminBlogPostRequest.UrlHandle;
            blogPost.PublishedDate = DateTime.SpecifyKind(adminBlogPostRequest.PublishedDate, DateTimeKind.Utc);
            blogPost.Author = adminBlogPostRequest.Author;
            blogPost.Visiable = adminBlogPostRequest.Visiable;

            // Update tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in adminBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            blogPost.Tags = selectedTags;

            await blogPostRepository.UpdateAsync(blogPost);

            return RedirectToAction("List");
        }
    }
}