using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class EditPhotoEventModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;

        [BindProperty]
        public PhotoEvent? PhotoEventToUpdate { get; set; }

        public EditPhotoEventModel(IRepositoryAsync<PhotoEvent> repo)
        {
            _repo = repo;
        }

        public async Task OnGet(int id)
        {
            PhotoEventToUpdate = await _repo.GetAsync(id);
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                await _repo.UpdateAsync(PhotoEventToUpdate);
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
