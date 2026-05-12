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
        public List<int> ParentsId { get; set; }


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

            //if (!ModelState.IsValid || ParentsId == null || ParentsId.Count == 0 || SchoolClassId < 1)
            //{
            //    return Page();
            //}
            NewStudent.Parents = new List<Parent>();
            foreach (var parentId in ParentsId)
            {
                var parent = await _parentRepo.GetAsync(parentId);
                if (parent != null)
                {
                    NewStudent.Parents.Add(parent);
                }
            }
            try
            {
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

