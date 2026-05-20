using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Students
{
    public class DeleteStudentModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<Student> _repo;
        #endregion

        #region Properties
        public Student DeleteStudent { get; set; }
        #endregion

        #region Constructors
        public DeleteStudentModel(IRepositoryAsync<Student> repo)
        {
            _repo = repo;

        }
        #endregion

        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteStudent = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteStudent = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteStudent);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        } 
        #endregion
    }
}

