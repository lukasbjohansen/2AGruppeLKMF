using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Teachers
{
    public class DeleteTeacherModel : PageModel
    {
        private readonly ITeacherRepository _repo;

        public Teacher DeleteTeacher { get; set; }

        public DeleteTeacherModel(ITeacherRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeleteTeacher = await _repo.GetAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteTeacher = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteTeacher);
            return RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}