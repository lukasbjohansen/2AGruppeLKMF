using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class CreateParentModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        [BindProperty]
        public Parent NewParent { get; set; }

        public CreateParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        public void OnGet()
        {
            NewParent = new Parent();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewParent);
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
