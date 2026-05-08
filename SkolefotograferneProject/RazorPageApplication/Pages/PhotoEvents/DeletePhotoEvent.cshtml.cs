using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class DeletePhotoEventModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;
        public PhotoEvent DeletePhotoEvent { get; set; }

        public DeletePhotoEventModel(IRepositoryAsync<PhotoEvent> repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeletePhotoEvent = await _repo.GetAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeletePhotoEvent = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeletePhotoEvent);
            return RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
