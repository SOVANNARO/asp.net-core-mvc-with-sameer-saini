using asp_net_core_mvc.Models.Domain;
using asp_net_core_mvc.Models.ViewModels;
using asp_net_core_mvc.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_mvc.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName,
                };
                await tagRepository.AddAsync(tag);
                return RedirectToAction("List");
            }
            return View(addTagRequest);
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);
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
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Id = editTagRequest.Id,
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName,
                };
                var updateTag = await tagRepository.UpdateAsync(tag);
                if (updateTag != null)
                {
                    return RedirectToAction("List");
                }
            }
            return View(editTagRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedTag = await tagRepository.DeleteAsync(id);
            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }
            return NotFound();
        }
    }
}