using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class EditSchoolModel : PageModel
    {
        private readonly IRepositoryAsync<School> _repo;

        [BindProperty]
        public School? SchoolToUpdate { get; set; }

        public EditSchoolModel(IRepositoryAsync<School> repo)
        {
            _repo = repo;
        }

        public async Task OnGet(int id)
        {
            SchoolToUpdate = await _repo.GetAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.UpdateAsync(SchoolToUpdate);
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
