using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace RazorPageApplication.Pages.Secretaries
{
    public class CreateSecretaryModel : PageModel
    {
        private IRepositoryAsync<Secretary> _secretaryRepo;
        private IRepositoryAsync<School> _schoolRepo;
        
        [BindProperty]
        public Secretary NewSecretary { get; set; }

        [BindProperty]
        public int SchoolID { get; set; }
        public CreateSecretaryModel(IRepositoryAsync<Secretary> secretaryRepo, IRepositoryAsync<School> schoolRepo)
        {
            _secretaryRepo = secretaryRepo;
            _schoolRepo = schoolRepo;
        }
        public void OnGet()
        {
           NewSecretary = new Secretary();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                NewSecretary.School = await _schoolRepo.GetAsync(SchoolID);
                await _secretaryRepo.CreateAsync(NewSecretary);
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
