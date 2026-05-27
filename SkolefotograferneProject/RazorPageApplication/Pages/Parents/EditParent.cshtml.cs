using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class EditParentModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<Parent> _repo;
        #endregion

        #region Properties
        [BindProperty]
        public Parent? ParentToUpdate { get; set; }
        #endregion

        #region Constructors
        public EditParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        #endregion

        #region Methods
        public async Task OnGet(int id)
        {
            ParentToUpdate = await _repo.GetAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.UpdateAsync(ParentToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        } 
        #endregion
    }
}
