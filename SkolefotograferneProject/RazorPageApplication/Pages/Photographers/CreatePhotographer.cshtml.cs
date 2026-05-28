using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photographers
{
    public class CreatePhotographerModel : PageModel
    {
        #region Instance Fields
        private readonly IRepositoryAsync<Photographer> _repo;
        #endregion

        #region Properties
        [BindProperty]
        public Photographer NewPhotographer { get; set; }
        #endregion

        #region Constructors
        public CreatePhotographerModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }
        #endregion

        #region Methods
        public void OnGet()
        {
            NewPhotographer = new Photographer();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.CreateAsync(NewPhotographer);
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
