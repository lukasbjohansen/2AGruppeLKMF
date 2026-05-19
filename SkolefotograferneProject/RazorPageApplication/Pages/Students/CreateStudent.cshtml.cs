using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Required(ErrorMessage = "Skoleklasse er påkrævet")]
        public string SchoolClassId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Forælder er påkrævet")]
        public string ParentId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> ParentSelect { get; set; }

        public CreateStudentModel(IRepositoryAsync<Student> studentRepository, IRepositoryAsync<SchoolClass> schoolClassRepository, IRepositoryAsync<Parent> parentRepository)
        {
            _studentRepo = studentRepository;
            _schoolClassRepo = schoolClassRepository;
            _parentRepo = parentRepository;
        }
        public async Task OnGet()
        {
            NewStudent = new Student();

            List<SchoolClass> schoolClasses = await _schoolClassRepo.GetAllAsync();
            List<Parent> parents = await _parentRepo.GetAllAsync();

            SchoolClassSelect = schoolClasses.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Year}" });
            ParentSelect = parents.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Username}" });

        }
        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }

            try
            {
                NewStudent.SchoolClass = await _schoolClassRepo.GetAsync(Convert.ToInt32(SchoolClassId));
                NewStudent.Parent = await _parentRepo.GetAsync(Convert.ToInt32(ParentId));
                await _studentRepo.CreateAsync(NewStudent);
                return RedirectToPage("Index");
            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGet();
                return Page();

            }

        }
    }
}

