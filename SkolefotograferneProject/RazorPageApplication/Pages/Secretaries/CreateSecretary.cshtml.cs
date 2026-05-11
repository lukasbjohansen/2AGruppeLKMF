using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace RazorPageApplication.Pages.Secretaries
{
    public class CreateSecretaryModel : PageModel
    {
        private IRepositoryAsync<Secretary> _repo;
        [BindProperty]
        public Secretary NewSecretary { get; set; }
        public CreateSecretaryModel(IRepositoryAsync<Secretary> secretaryRepo)
        {
            _repo = secretaryRepo;
        }
        public void OnGet()
        {
           NewSecretary = new Secretary();
        }
        public async Task<IActionResult> OnPost() //DER ER EN FEJL VED VALIDERING
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewSecretary);
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
