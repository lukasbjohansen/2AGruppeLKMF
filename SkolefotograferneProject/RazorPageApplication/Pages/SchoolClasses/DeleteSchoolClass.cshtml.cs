using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class DeleteSchoolClassModel : PageModel
    {
        private readonly IRepositoryAsync<SchoolClass> _repo;
       
        public SchoolClass DeleteSchoolClass { get; set; }
        public DeleteSchoolClassModel(IRepositoryAsync<SchoolClass> repo)
        {
            _repo = repo;
            
        }

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
    }
}
