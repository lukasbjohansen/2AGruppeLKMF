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
        #region Instance fields
        /// <summary>
        /// _repo instance field that represents the repository for the Student model.
        /// </summary>
        private readonly IRepositoryAsync<Student> _studentRepo;

        /// <summary>
        /// _parentRepo instance field that represents the repository for the Parent model.
        /// </summary>
        private readonly IRepositoryAsync<Parent> _parentRepo;

        /// <summary>
        /// _schoolClassRepo instance field that represents the repository for the SchoolClass model.
        /// </summary>
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;

        #endregion

        #region Properties
        /// <summary>
        /// StudentToUpdate property that represents the student that is being edited on the edit student page.
        /// </summary>
        [BindProperty]
        public Student StudentToUpdate { get; set; }

        /// <summary>
        /// ParentId property that represents the id of the parent that is selected in the dropdown menu on the edit student page.
        /// </summary>
        [BindProperty]
        [Required]
        public string ParentId { get; set; }

        /// <summary>
        /// SchoolClassId property that represents the id of the school class that is selected in the dropdown menu on the edit student page.
        /// </summary>
        [BindProperty]
        [Required]
        public string SchoolClassId { get; set; }

        /// <summary>
        /// SchoolClassSelect property that represents the list of school classes that will be displayed in the dropdown menu on the edit student page.
        /// </summary>
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }

        /// <summary>
        /// ParentSelect property that represents the list of parents that will be displayed in the dropdown menu on the edit student page.
        /// </summary>
        [BindProperty]
        public IEnumerable<SelectListItem> ParentSelect { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// EditStudentModel constructor that is used to create a new instance of the EditStudentModel class with the repositories for the Student, Parent, and SchoolClass models injected as parameters.
        /// </summary>
        public EditStudentModel(IRepositoryAsync<Student> studentRepo, IRepositoryAsync<Parent> teacherRepo, IRepositoryAsync<SchoolClass> schoolRepo)
        {
            _studentRepo = studentRepo;
            _parentRepo = teacherRepo;
            _schoolClassRepo = schoolRepo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// OnGet method that is called when the user navigates to the edit student page.
        /// </summary>
        public async Task OnGet(int id)
        {
            StudentToUpdate = await _studentRepo.GetAsync(id);
            List<SchoolClass> schoolClasses = await _schoolClassRepo.GetAllAsync();
            List<Parent> parents = await _parentRepo.GetAllAsync();

            SchoolClassSelect = schoolClasses.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Year}" });
            ParentSelect = parents.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Mail}" });

            SchoolClassId = StudentToUpdate.SchoolClass.Id.ToString();
            ParentId = StudentToUpdate.Parent.Id.ToString();
        }
        /// <summary>
        /// OnPostUpdate method that is called when the user submits the form to update a student's information on the edit student page.
        /// </summary>
        /// <returns></returns>
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
                await _studentRepo.UpdateAsync(StudentToUpdate);
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
        #endregion
    } 
}

