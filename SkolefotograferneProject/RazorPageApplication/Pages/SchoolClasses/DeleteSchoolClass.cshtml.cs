using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class DeleteSchoolClassModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<SchoolClass> _repo;
        #endregion

        #region Properties
        public SchoolClass DeleteSchoolClass { get; set; }
        #endregion

        #region Constructors
        public DeleteSchoolClassModel(IRepositoryAsync<SchoolClass> repo)
        {
            _repo = repo;

        }
        #endregion

        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteSchoolClass = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteSchoolClass = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteSchoolClass);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        } 
        #endregion
    }
}
