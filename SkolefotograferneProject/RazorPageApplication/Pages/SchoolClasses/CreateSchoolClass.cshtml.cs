using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class CreateSchoolClassModel : PageModel
    {

        #region Instance fields
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        private ITeacherRepository _teacherRepo;
        private IRepositoryAsync<School> _schoolRepo;
        #endregion

        #region Propeties
        [BindProperty]
        public SchoolClass NewSchoolClass { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Lærer er påkrævet")]
        public string TeacherId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Skole er påkrævet")]
        public string SchoolId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> TeacherSelect { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }
        #endregion

        #region Constructors
        public CreateSchoolClassModel(IRepositoryAsync<SchoolClass> schoolClassRepository, ITeacherRepository teacherRepository, IRepositoryAsync<School> schoolRepository)
        {
            _schoolClassRepo = schoolClassRepository;
            _teacherRepo = teacherRepository;
            _schoolRepo = schoolRepository;
        }
        #endregion

        #region Methods
        public async Task OnGet()
        {
            NewSchoolClass = new SchoolClass();

            List<Teacher> teachers = await _teacherRepo.GetAllAsync();
            List<School> schools = await _schoolRepo.GetAllAsync();

            TeacherSelect = teachers.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Mail}" });
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.PostalCode}" });

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
                NewSchoolClass.School = await _schoolRepo.GetAsync(Convert.ToInt32(SchoolId));
                NewSchoolClass.Teacher = await _teacherRepo.GetAsync(Convert.ToInt32(TeacherId));
                await _schoolClassRepo.CreateAsync(NewSchoolClass);
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
        #endregion
    }
}
