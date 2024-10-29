using asp_net_core_mvc.Data;
using asp_net_core_mvc.Models.Domain;
using asp_net_core_mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_mvc.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            var tags = bloggieDbContext.Tags.ToList();
            return View(tags);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = bloggieDbContext.Tags.FirstOrDefault(t => t.Id == id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = bloggieDbContext.Tags.FirstOrDefault(t => t.Id == editTagRequest.Id);
            if (tag != null)
            {
                tag.Name = editTagRequest.Name;
                tag.DisplayName = editTagRequest.DisplayName;
                bloggieDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var tag = bloggieDbContext.Tags.FirstOrDefault(t => t.Id == id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                bloggieDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return NotFound();
        }
    }
}