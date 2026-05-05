using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class DeleteSchoolModel : PageModel
    {
        private readonly ISchoolRepositoryAsync _repo;
        public School DeleteSchool { get; set; }
        public DeleteSchoolModel (ISchoolRepositoryAsync repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeleteSchool = await _repo.GetSchoolAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteSchool = await _repo.GetSchoolAsync(id);
            await _repo.DeleteSchoolAsync(DeleteSchool);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
