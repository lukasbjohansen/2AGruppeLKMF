using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Students
{
    public class DeleteStudentModel : PageModel
    {
        #region Instance fields
        /// <summary>
        /// _studentRepo instance field that represents the repository for the Student model. This field is used to access the methods of the repository to retrieve and manipulate student data from the database when the delete student page is accessed.
        /// </summary>
        private IRepositoryAsync<Student> _studentRepo;
        #endregion

        #region Properties
        /// <summary>
        /// DeleteStudent property that represents the student that is being deleted on the delete student page. This property is used to display the details of the student that is being deleted on the user interface and to pass the student
        /// </summary>
        public Student DeleteStudent { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// DeleteStudentModel constructor that takes an IRepositoryAsync<Student> parameter and assigns it to the _studentRepo instance field. This allows the DeleteStudentModel class to access the methods of the repository to retrieve and manipulate student data from the database when the delete student page is accessed.
        /// </summary>
        public DeleteStudentModel(IRepositoryAsync<Student> studentRepo)
        {
            _studentRepo = studentRepo;

        }
        #endregion

        #region Methods
        /// <summary>
        /// Onget method that takes an int id parameter and retrieves the student with the specified id from the database using the GetAsync method of the _studentRepo repository. The retrieved student is then assigned to the DeleteStudent property, which is used to display the details of the student on the delete student page. Finally, the method returns the Page() method to render the delete student page with the retrieved student details.
        /// </summary>
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteStudent = await _studentRepo.GetAsync(id);
            return Page();
        }

        /// <summary>
        /// OnPostDelete method that takes an int id parameter and retrieves the student with the specified id from the database using the GetAsync method of the _studentRepo repository. The retrieved student is then assigned to the DeleteStudent property, which is used to delete the student from the database using the DeleteAsync method of the _studentRepo repository. Finally, the method redirects the user to the index page of the students section using the RedirectToPage() method.
        /// </summary>
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteStudent = await _studentRepo.GetAsync(id);
            await _studentRepo.DeleteAsync(DeleteStudent);
            return RedirectToPage("Index");
        }

        /// <summary>
        /// OnPost method that is called when the user clicks the cancel button on the delete student page. This method simply redirects the user back to the index page of the students section using the RedirectToPage() method, without performing any deletion of student data from the database.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        } 
        #endregion
    }
}

