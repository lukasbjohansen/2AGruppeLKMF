using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Secretaries
{
    public class EditSecretaryModel : PageModel
    {
        private readonly IRepositoryAsync<Secretary> _secretaryRepo;
        private readonly IRepositoryAsync<School> _schoolRepo;

        [BindProperty]
        public Secretary? SecretaryToUpdate { get; set; }

        [BindProperty]
        public int SchoolID { get; set; }

        public EditSecretaryModel(IRepositoryAsync<Secretary> repo, IRepositoryAsync<School>schoolRepo)
        {
            _secretaryRepo = repo;
            _schoolRepo = schoolRepo;
        }
        public async Task OnGet(int id)
        {
            SecretaryToUpdate = await _secretaryRepo.GetAsync(id);
            SchoolID = SecretaryToUpdate.School.Id;
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                SecretaryToUpdate.School = await _schoolRepo.GetAsync(SchoolID);
                await _secretaryRepo.UpdateAsync(SecretaryToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}
