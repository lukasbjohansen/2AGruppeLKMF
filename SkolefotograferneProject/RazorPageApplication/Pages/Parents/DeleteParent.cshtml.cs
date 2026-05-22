using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class DeleteParentModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<Parent> _repo;
        #endregion
        #region Properties
        public Parent DeleteParent { get; set; }
        #endregion

        #region Constructors
        public DeleteParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            DeleteParent = await _repo.GetAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteParent = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteParent);
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        } 
        #endregion
    }
}
