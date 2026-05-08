using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class CreatePhotoEventModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;

        [BindProperty]
        public PhotoEvent NewPhotoEvent { get; set; }

        public CreatePhotoEventModel(IRepositoryAsync<PhotoEvent> repo)
        {
            _repo = repo;
        }

        public void OnGet()
        {
            NewPhotoEvent = new PhotoEvent();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewPhotoEvent);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
