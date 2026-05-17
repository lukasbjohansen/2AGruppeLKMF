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

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            if (string.IsNullOrEmpty(FilterCriteria))
            {
                Photographers = await _repo.GetAllAsync();
            }
            else
            {
                Photographers = await _repo.FilterAsync(FilterCriteria);
            }
        }
    }
}
