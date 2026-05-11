using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Students
{
    public class DeleteStudentModel : PageModel
    {
        private readonly IRepositoryAsync<Student> _repo;

        public Student DeleteStudent { get; set; }
        public DeleteStudentModel(IRepositoryAsync<Student> repo)
        {
            _repo = repo;

        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeleteStudent = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteStudent = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteStudent);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
}
