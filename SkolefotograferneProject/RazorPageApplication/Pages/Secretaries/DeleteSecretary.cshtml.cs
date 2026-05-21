using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Secretaries
{
    public class DeleteSecretaryModel : PageModel
    {
        private readonly IRepositoryAsync<Secretary> _repo;
        public Secretary DeleteSecretary { get; set; }
        public DeleteSecretaryModel(IRepositoryAsync<Secretary> secretaryRepo)
        {
            _repo = secretaryRepo;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteSecretary = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteSecretary = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteSecretary);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
