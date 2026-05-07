using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photographers
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryAsync<Photographer> _repo;

        public List<Photographer> Photographers { get; set; }

        public IndexModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            Photographers = await _repo.GetAllAsync();
        }
    }
}
