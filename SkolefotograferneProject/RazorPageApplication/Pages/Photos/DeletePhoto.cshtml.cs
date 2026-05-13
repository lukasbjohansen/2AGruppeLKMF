using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photos
{
    public class DeletePhotoModel : PageModel
    {
        private IRepositoryAsync<Photo> _repo;

        public Photo DeletePhoto { get; set; }
        public DeletePhotoModel(IRepositoryAsync<Photo> photoRepository)
        {
            _repo = photoRepository;

        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeletePhoto = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeletePhoto = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeletePhoto);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
