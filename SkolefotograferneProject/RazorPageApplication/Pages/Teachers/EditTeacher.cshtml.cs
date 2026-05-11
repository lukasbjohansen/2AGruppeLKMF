using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Teachers
{
    public class EditTeacherModel : PageModel
    {
        private readonly ITeacherRepository _repo;

        [BindProperty]
        public Teacher? TeacherToUpdate { get; set; }

        public EditTeacherModel(ITeacherRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGet(int id)
        {
            TeacherToUpdate = await _repo.GetAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TeacherToUpdate.Password))
                {
                    Teacher? existing = await _repo.GetAsync(TeacherToUpdate.Id);
                    if (existing != null)
                    {
                        TeacherToUpdate.Password = existing.Password;
                    }
                    ModelState.Remove("TeacherToUpdate.Password");
                }
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                await _repo.UpdateAsync(TeacherToUpdate);
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