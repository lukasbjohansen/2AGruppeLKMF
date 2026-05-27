using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class CreateParentModel : PageModel
    {
        #region Instance fields
        /// <summary>
        /// The repository field for the Parent entity, which will be used to perform asynchronous operations such as creating a new parent record in the database.
        /// </summary>
        private IRepositoryAsync<Parent> _repo; 
        #endregion

        #region Properties
        [BindProperty]
        public Parent NewParent { get; set; } 
        #endregion

        #region Constructors
        public CreateParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        } 
        #endregion
        #region Methods
        public void OnGet()
        {
            NewParent = new Parent();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewParent);
                return RedirectToPage("Index");
            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

        } 
        #endregion
    }
}
