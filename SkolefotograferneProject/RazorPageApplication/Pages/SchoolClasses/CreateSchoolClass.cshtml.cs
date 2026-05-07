using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class CreateSchoolClassModel : PageModel
    {

        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        private IRepositoryAsync<Teacher> _teacherRepo;
        private IRepositoryAsync<School> _schoolRepo;

        [BindProperty]
        public SchoolClass NewSchoolClass { get; set; }

        [BindProperty]
        [Required]
        public int TeacherId { get; set; }

        [BindProperty]
        [Required]
        public int SchoolId { get; set; }

        public CreateSchoolClassModel(IRepositoryAsync<SchoolClass> schoolClassRepository,IRepositoryAsync<Teacher>teacherRepository,IRepositoryAsync<School>schoolRepository)
        {
            _schoolClassRepo = schoolClassRepository;
            _teacherRepo = teacherRepository;
            _schoolRepo = schoolRepository;
        }
        public void OnGet()
        {
            NewSchoolClass = new SchoolClass();
        }
        public async Task<IActionResult> OnPost()
        {
            
            if (!ModelState.IsValid||TeacherId<1||SchoolId<1)
            {
                return Page();
            }
            NewSchoolClass.School = await _schoolRepo.GetAsync(SchoolId);
            NewSchoolClass.Teacher = await _teacherRepo.GetAsync(TeacherId);
            try
            {
                await _schoolClassRepo.CreateAsync(NewSchoolClass);
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
