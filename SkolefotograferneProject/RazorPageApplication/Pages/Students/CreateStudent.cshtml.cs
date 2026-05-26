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
        #region Instance fields
        /// <summary>
        /// _studentRepo instance field that represents the repository for the Student model. This field is used to access the methods of the repository to create new student data in the database when the create student page is accessed.
        /// </summary>
        private IRepositoryAsync<Student> _studentRepo;

        /// <summary>
        /// _schoolClassRepo instance field that represents the repository for the SchoolClass model. This field is used to access the methods of the repository to retrieve school class data from the database when the create student page is accessed, and to assign a school class to the new student being created.
        /// </summary>
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;

        /// <summary>
        /// _parentRepo instance field that represents the repository for the Parent model. This field is used to access the methods of the repository to retrieve parent data from the database when the create student page is accessed, and to assign a parent to the new student being created.
        /// </summary>
        private IRepositoryAsync<Parent> _parentRepo;
        #endregion

        #region Properties
        /// <summary>
        /// NewStudent property that represents the student that is being created on the create student page. This property is used to bind the form data entered by the user on the create student page to a new instance of the Student model, which is then used to create a new student record in the database when the form is submitted.
        /// </summary>
        [BindProperty]
        public Student NewStudent { get; set; }

        /// <summary>
        /// SchoolClassId property that represents the id of the school class that is selected in the dropdown menu on the create student page. This property is used to bind the selected school class id from the form data entered by the user on the create student page to a string property, which is then used to retrieve the corresponding SchoolClass object from the database when the form is submitted.
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "Skoleklasse er påkrævet")]
        public string SchoolClassId { get; set; }

        /// <summary>
        /// ParentId property that represents the id of the parent that is selected in the dropdown menu on the create student page. This property is used to bind the selected parent id from the form data entered by the user on the create student page to a string property, which is then used to retrieve the corresponding Parent object from the database when the form is submitted.
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "Forælder er påkrævet")]
        public string ParentId { get; set; }

        /// <summary>
        /// SchoolClassSelect property that represents the list of school classes that will be displayed in the dropdown menu on the create student page. This property is used to bind the list of school classes retrieved from the database to an IEnumerable<SelectListItem> property, which is then used to populate the dropdown menu on the create student page with the available school classes for selection.
        /// </summary>
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }

        /// <summary>
        /// ParentSelect property that represents the list of parents that will be displayed in the dropdown menu on the create student page. This property is used to bind the list of parents retrieved from the database to an IEnumerable<SelectListItem> property, which is then used to populate the dropdown menu on the create student page with the available parents for selection.
        /// </summary>
        [BindProperty]
        public IEnumerable<SelectListItem> ParentSelect { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// CreateStudentModel constructor that is used to create a new instance of the CreateStudentModel class with the repositories for the Student, SchoolClass, and Parent models injected as parameters. This allows the CreateStudentModel class to access the methods of the repositories to retrieve and manipulate student, school class, and parent data from the database when the create student page is accessed.
        /// </summary>
        public CreateStudentModel(IRepositoryAsync<Student> studentRepository, IRepositoryAsync<SchoolClass> schoolClassRepository, IRepositoryAsync<Parent> parentRepository)
        {
            _studentRepo = studentRepository;
            _schoolClassRepo = schoolClassRepository;
            _parentRepo = parentRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// OnGet method that is called when the create student page is accessed. This method initializes the NewStudent property with a new instance of the Student model, retrieves the list of school classes and parents from the database using the GetAllAsync method of the respective repositories, and populates the SchoolClassSelect and ParentSelect properties with the retrieved data to be used in the dropdown menus on the create student page for selecting a school class and a parent for the new student being created.
        /// </summary>
        public async Task OnGet()
        {
            NewStudent = new Student();

            List<SchoolClass> schoolClasses = await _schoolClassRepo.GetAllAsync();
            List<Parent> parents = await _parentRepo.GetAllAsync();

            SchoolClassSelect = schoolClasses.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Year}" });
            ParentSelect = parents.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Id} - {p.Name} - {p.Mail}" });

        }

        /// <summary>
        /// OnPost method that is called when the form on the create student page is submitted. This method first checks if the model state is valid, and if not, it calls the OnGet method to reinitialize the page and returns the Page() method to render the create student page with validation error messages. If the model state is valid, it retrieves the selected school class and parent from the database using the GetAsync method of the respective repositories based on the selected ids from the form data, assigns them to the NewStudent property, and then creates a new student record in the database using the CreateAsync method of the _studentRepo repository. Finally, it redirects the user to the index page of the students section using the RedirectToPage() method. If any exceptions occur during this process, it catches the exception, sets an error message in ViewData, adds a model error to display the error message on the page, calls the OnGet method to reinitialize the page, and returns the Page() method to render the create student page with the error message displayed.
        /// </summary>
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
        #endregion
    }
}

