using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }

        public EditSecretaryModel(IRepositoryAsync<Secretary> repo, IRepositoryAsync<School>schoolRepo)
        {
            _secretaryRepo = repo;
            _schoolRepo = schoolRepo;
        }
        public async Task OnGet(int id)
        {
            List<School> schools = await _schoolRepo.GetAllAsync();
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.PostalCode}" });
            SecretaryToUpdate = await _secretaryRepo.GetAsync(id);
            SchoolID = SecretaryToUpdate.School.Id;
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(SecretaryToUpdate.Id); //korrekt?
                return Page();
            }
            try
            {
                SecretaryToUpdate.School = await _schoolRepo.GetAsync(Convert.ToInt32(SchoolID));
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
