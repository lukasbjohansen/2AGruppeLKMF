using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class DeleteParentModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        public Parent DeleteParent { get; set; }

        public DeleteParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteParent = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteParent = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteParent);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
