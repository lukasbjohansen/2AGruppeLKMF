using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class EditSchoolClassModel : PageModel
    {
        private readonly IRepositoryAsync<SchoolClass> _repo;
        private readonly IRepositoryAsync<Teacher> _teacherRepo;
        private readonly IRepositoryAsync<School> _schoolRepo;

        [BindProperty]
        public SchoolClass? SchoolClassToUpdate { get; set; }

        [BindProperty]
        [Required]
        public int TeacherId { get; set; }

        [BindProperty]
        [Required]
        public int SchoolId { get; set; }

        public EditSchoolClassModel(IRepositoryAsync<SchoolClass> repo,IRepositoryAsync<Teacher> teacherRepo,IRepositoryAsync<School> schoolRepo)
        {
            _repo = repo;
            _teacherRepo = teacherRepo;
            _schoolRepo = schoolRepo;
        }

        public async Task OnGet(int id)
        {
            SchoolClassToUpdate = await _repo.GetAsync(id);

            TeacherId = SchoolClassToUpdate.Teacher.Id;
            SchoolId = SchoolClassToUpdate.School.Id;

        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid || TeacherId < 1 || SchoolId < 1)
            {
                return Page();
            }
                var teacher = await _teacherRepo.GetAsync(TeacherId);
                var school = await _schoolRepo.GetAsync(SchoolId);

            SchoolClassToUpdate.Teacher = teacher;
            SchoolClassToUpdate.School = school;

            try
            {
                await _repo.UpdateAsync(SchoolClassToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}

