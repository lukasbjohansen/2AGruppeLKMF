using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photographers
{
    public class EditPhotographerModel : PageModel
    {
        private readonly IRepositoryAsync<Photographer> _repo;

        [BindProperty]
        public Photographer? PhotographerToUpdate { get; set; }

        public EditPhotographerModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }

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
    }
}
