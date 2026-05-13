using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.Students
{
    public class CreateStudentModel : PageModel
    {
        private IRepositoryAsync<Student> _studentRepo;
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        private IRepositoryAsync<Parent> _parentRepo;

        [BindProperty]
        public Student NewStudent { get; set; }

        [BindProperty]
        [Required]
        public int SchoolClassId { get; set; }
        [BindProperty]
        [Required]
        public int ParentId { get; set; }


        public CreateStudentModel(IRepositoryAsync<Student> studentRepository, IRepositoryAsync<SchoolClass> schoolClassRepository, IRepositoryAsync<Parent> parentRepository)
        {
            _studentRepo = studentRepository;
            _schoolClassRepo = schoolClassRepository;
            _parentRepo = parentRepository;
        }
        public void OnGet()
        {
            NewStudent = new Student();
        }
        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid || ParentId<1||SchoolClassId<1)
            {
                return Page();
            }

            try
            {
                NewStudent.Parent = await _parentRepo.GetAsync(ParentId);
                NewStudent.SchoolClass = await _schoolClassRepo.GetAsync(SchoolClassId);
                await _studentRepo.CreateAsync(NewStudent);
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

