using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class CreateSchoolModel : PageModel
    {
        private ISchoolRepositoryAsync _repo;

        [BindProperty]
        public School NewSchool { get; set; }

        public CreateSchoolModel(ISchoolRepositoryAsync schoolRepository)
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
                await _repo.CreateSchoolAsync(NewSchool);
                return RedirectToPage("Index");
            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            
        }

    }
}
