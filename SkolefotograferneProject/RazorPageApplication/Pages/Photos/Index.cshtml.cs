using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photos
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Photo> _repo;

        public List<Photo> Photos { get; set; }
        public IndexModel(IRepositoryAsync<Photo> photoRepository)
        {
            _repo = photoRepository;
        }
        public async Task OnGet()
        {
            Photos = await _repo.GetAllAsync();
        }
    }
}
