using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photographers
{
    public class DeletePhotographerModel : PageModel
    {
        private readonly IRepositoryAsync<Photographer> _repo;
        public Photographer DeletePhotographer { get; set; }

        public DeletePhotographerModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeletePhotographer = await _repo.GetAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeletePhotographer = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeletePhotographer);
            return RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
