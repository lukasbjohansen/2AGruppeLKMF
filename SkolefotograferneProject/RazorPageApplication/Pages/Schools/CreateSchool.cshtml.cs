using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;

namespace RazorPageApplication.Pages.Schools
{
    public class CreateSchoolModel : PageModel
    {
        private IRepository<School> _repo;

        [BindProperty]
        public School NewSchool { get; set; }

        public CreateSchoolModel(IRepository<School> schoolRepository)
        {
            _repo = schoolRepository;
        }
        public void OnGet()
        {
            NewSchool = new School();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewSchool);
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
