using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.Students
{
    public class EditStudentModel : PageModel
    {
        private readonly IRepositoryAsync<Student> _repo;
        private readonly IRepositoryAsync<Parent> _parentRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;

        [BindProperty]
        public Student StudentToUpdate { get; set; }

        [BindProperty]
        [Required]
        public string ParentId { get; set; }

        [BindProperty]
        [Required]
        public string SchoolClassId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> ParentSelect { get; set; }

        public EditStudentModel(IRepositoryAsync<Student> repo, IRepositoryAsync<Parent> teacherRepo, IRepositoryAsync<SchoolClass> schoolRepo)
        {
            _repo = repo;
            _parentRepo = teacherRepo;
            _schoolClassRepo = schoolRepo;
        }

        public async Task OnGet(int id)
        {
            StudentToUpdate = await _repo.GetAsync(id);
            List<SchoolClass> schoolClasses = await _schoolClassRepo.GetAllAsync();
            List<Parent> parents = await _parentRepo.GetAllAsync();

            SchoolClassSelect = schoolClasses.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Year}" });
            ParentSelect = parents.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Mail}" });

            SchoolClassId = StudentToUpdate.SchoolClass.Id.ToString();
            ParentId = StudentToUpdate.Parent.Id.ToString();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(StudentToUpdate.Id);
                return Page();
            }

            try
            {
                StudentToUpdate.SchoolClass = await _schoolClassRepo.GetAsync(Convert.ToInt32(SchoolClassId));
                StudentToUpdate.Parent = await _parentRepo.GetAsync(Convert.ToInt32(ParentId));
                await _repo.UpdateAsync(StudentToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                await OnGet(StudentToUpdate.Id);
                return Page();
            }
        }
    }
}

