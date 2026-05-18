using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Globalization;

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

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }

        public IndexModel(IRepositoryAsync<Photographer> repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            List<Photographer> allPhotographers = await _repo.GetAllAsync();
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {
                    case "PhotographerName":
                        Photographers = allPhotographers
                            .Where(p => p.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "Mail":
                        Photographers = allPhotographers
                            .Where(p => p.Mail.Contains(FilterCriteria))
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
                                p.Mail.Contains(FilterCriteria) ||
                                p.Password.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                p.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                p.CVR.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }
            }
            else
            {
                Photographers = allPhotographers;
            }
            if (!string.IsNullOrEmpty(SortBy))
            {
                Photographers = allPhotographers;
                SortPhotographers();
            }
        }

        public void SortPhotographers()
        {
            switch (SortBy)
            {
                case "Id":
                    Photographers.Sort(new GenericComparer<Photographer, int>(p => p.Id, IsDescending));
                    break;
                case "Name":
                    Photographers.Sort(new GenericComparer<Photographer, string>(p => p.Name, IsDescending));
                    break;
                case "Mail":
                    Photographers.Sort(new GenericComparer<Photographer, string>(p => p.Mail, IsDescending));
                    break;
                case "Password":
                    Photographers.Sort(new GenericComparer<Photographer, string>(p => p.Password, IsDescending));
                    break;
                case "PhoneNumber":
                    Photographers.Sort(new GenericComparer<Photographer, string>(p => p.PhoneNumber, IsDescending));
                    break;
                case "CVR":
                    Photographers.Sort(new GenericComparer<Photographer, string>(p => p.CVR, IsDescending));
                    break;
            }
        }
    }
}
