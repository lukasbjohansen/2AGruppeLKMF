using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.Students
{
    public class EditStudentModel : PageModel
    {
        private readonly IRepositoryAsync<Student> _repo;
      private readonly IRepositoryAsync<Parent> _teacherRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolRepo;

        [BindProperty]
        public Student? StudentToUpdate { get; set; }

        [BindProperty]
        [Required]
        public int ParentId { get; set; }

        [BindProperty]
        [Required]
        public int SchoolClassId { get; set; }

        public EditStudentModel(IRepositoryAsync<Student> repo, IRepositoryAsync<Parent> teacherRepo, IRepositoryAsync<SchoolClass> schoolRepo)
        {
            _repo = repo;
            _teacherRepo = teacherRepo;
            _schoolRepo = schoolRepo;
        }

        public async Task OnGet(int id)
        {
            StudentToUpdate = await _repo.GetAsync(id);
            if (StudentToUpdate != null)
            {
                ParentId = StudentToUpdate.Parents.FirstOrDefault()?.Id ?? 0;
                SchoolClassId = StudentToUpdate.SchoolClass?.Id ?? 0;
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            //if (!ModelState.IsValid || TeacherId < 1 || SchoolId < 1)
            //{
            //    return Page();
            //}
            StudentToUpdate.Parents = await _teacherRepo.GetAllAsync();
            StudentToUpdate.SchoolClass = await _schoolRepo.GetAsync(SchoolClassId);

            try
            {
                await _repo.UpdateAsync(StudentToUpdate);
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
}
