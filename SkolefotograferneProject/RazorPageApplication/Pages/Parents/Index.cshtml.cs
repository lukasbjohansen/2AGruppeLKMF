using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Parents
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        public List<Parent> Parents { get; set; }
        public IndexModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        public async Task OnGet()
        {
            Parents = await _repo.GetAllAsync();
        }
    }
}
