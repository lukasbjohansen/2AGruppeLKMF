using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class CreateSchoolClassModel : PageModel
    {
        
        private IRepositoryAsync<SchoolClass> _repo;

        [BindProperty]
        public SchoolClass NewSchoolClass { get; set; }

        public CreateSchoolClassModel(IRepositoryAsync<SchoolClass> schoolClassRepository)
        {
            _repo = schoolClassRepository;
        }
        public void OnGet()
        {
            NewSchoolClass = new SchoolClass();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewSchoolClass);
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
