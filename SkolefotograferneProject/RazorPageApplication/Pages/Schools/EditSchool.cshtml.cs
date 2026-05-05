using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class EditSchoolModel : PageModel
    {
        private readonly ISchoolRepositoryAsync _repo;

        [BindProperty]
        public School? SchoolToUpdate { get; set; }

        public EditSchoolModel(ISchoolRepositoryAsync repo)
        {
            _repo = repo;
        }

        public async Task OnGet(int id)
        {
            SchoolToUpdate = await _repo.GetSchoolAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.UpdateSchoolAsync(SchoolToUpdate);
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
