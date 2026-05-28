using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photographers
{
    public class EditPhotographerModel : PageModel
    {
        #region Instance Fields
        private readonly IRepositoryAsync<Photographer> _repo;
        #endregion

        #region Properties
        [BindProperty]
        public Photographer? PhotographerToUpdate { get; set; }
        #endregion

        #region Constructors
        public EditPhotographerModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }
        #endregion

        #region Methods
        public async Task OnGet(int id)
        {
            PhotographerToUpdate = await _repo.GetAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.UpdateAsync(PhotographerToUpdate);
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
