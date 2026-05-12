using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Secretaries
{
    public class EditSecretaryModel : PageModel
    {
        private readonly IRepositoryAsync<Secretary> _repo;

        [BindProperty]
        public Secretary? SecretaryToUpdate { get; set; }

        public EditSecretaryModel(IRepositoryAsync<Secretary> repo)
        {
            _repo = repo;
        }
        public async Task OnGet(int id)
        {
            SecretaryToUpdate = await _repo.GetAsync(id);
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                await _repo.UpdateAsync(SecretaryToUpdate);
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
