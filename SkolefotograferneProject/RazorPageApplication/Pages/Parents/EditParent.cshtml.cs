using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class EditParentModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        [BindProperty]
        public Parent? ParentToUpdate { get; set; }

        public EditParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }

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
    }
}
