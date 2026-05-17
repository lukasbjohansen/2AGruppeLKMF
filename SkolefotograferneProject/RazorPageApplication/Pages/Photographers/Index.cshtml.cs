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
            List<Photographer> allPhotographers = await _repo.GetAllAsync();
            if (string.IsNullOrEmpty(FilterCriteria))
            {
                Photographers = allPhotographers;
                return;
            }
            switch (FilterBy)
            {
                case "PhotographerName":
                    Photographers = allPhotographers
                        .Where(p => p.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "Mail":
                    Photographers = allPhotographers
                        .Where(p => p.Mail.ToString().Contains(FilterCriteria))
                        .ToList();
                    break;
                case "PhotographerPassword":
                    Photographers = allPhotographers
                        .Where(p => p.Password.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "PhoneNumber":
                    Photographers = allPhotographers
                        .Where(p => p.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "CVR":
                    Photographers = allPhotographers
                        .Where(p => p.CVR.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "All":
                    Photographers = allPhotographers
                        .Where(p =>
                            p.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.Mail.ToString().Contains(FilterCriteria) ||
                            p.Password.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.CVR.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
            }
        }
    }
}
