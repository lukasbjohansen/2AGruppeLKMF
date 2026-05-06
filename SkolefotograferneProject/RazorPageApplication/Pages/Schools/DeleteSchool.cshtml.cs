using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class DeleteSchoolModel : PageModel
    {
        private readonly IRepositoryAsync<School> _repo;
        public School DeleteSchool { get; set; }
        public DeleteSchoolModel (IRepositoryAsync<School> repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeleteSchool = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteSchool = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteSchool);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
