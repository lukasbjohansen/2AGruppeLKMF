using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;

        public List<PhotoEvent> PhotoEvents { get; set; }

        public IndexModel(IRepositoryAsync<PhotoEvent> repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            PhotoEvents = await _repo.GetAllAsync();
        }
    }
}
