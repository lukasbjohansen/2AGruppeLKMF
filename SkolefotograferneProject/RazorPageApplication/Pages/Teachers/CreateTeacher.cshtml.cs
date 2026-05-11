using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace RazorPageApplication.Pages.Teachers
{
    public class CreateTeacherModel : PageModel
    {
        private ITeacherRepository _repo;

        [BindProperty]
        public Teacher NewTeacher { get; set; }

        public CreateTeacherModel(ITeacherRepository teacherRepository)
        {
            _repo = teacherRepository;
        }

        public void OnGet()
        {
            NewTeacher = new Teacher();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewTeacher);
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