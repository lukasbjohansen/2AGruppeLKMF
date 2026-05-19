using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class EditSchoolClassModel : PageModel
    {
        private readonly IRepositoryAsync<SchoolClass> _repo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IRepositoryAsync<School> _schoolRepo;

        [BindProperty]
        public SchoolClass SchoolClassToUpdate { get; set; }

        [BindProperty]
        [Required]
        public string TeacherId { get; set; }

        [BindProperty]
        [Required]
        public string SchoolId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> TeacherSelect { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }

        public EditSchoolClassModel(IRepositoryAsync<SchoolClass> repo, ITeacherRepository teacherRepo,IRepositoryAsync<School> schoolRepo)
        {
            _repo = repo;
            _teacherRepo = teacherRepo;
            _schoolRepo = schoolRepo;
        }

        public async Task OnGet(int id)
        {
            SchoolClassToUpdate = await _repo.GetAsync(id)
                ;
            List<Teacher> teachers = await _teacherRepo.GetAllAsync();
            List<School> schools = await _schoolRepo.GetAllAsync();

            TeacherSelect = teachers.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Username}" });
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.PostalCode}" });


            SchoolId = SchoolClassToUpdate.School.Id.ToString();
            TeacherId = SchoolClassToUpdate.Teacher.Id.ToString();

        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(SchoolClassToUpdate.Id);
                return Page();
            }



            try
            {
                SchoolClassToUpdate.School = await _schoolRepo.GetAsync(Convert.ToInt32(SchoolId));
                SchoolClassToUpdate.Teacher = await _teacherRepo.GetAsync(Convert.ToInt32(TeacherId));
                await _repo.UpdateAsync(SchoolClassToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                await OnGet(SchoolClassToUpdate.Id);
                return Page();
            }
        }
    }
}

