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
                Photographers = FilterPhotographers(allPhotographers);
            }
            else
            {
                Photographers = allPhotographers;
            }
            if (!string.IsNullOrEmpty(SortBy))
            {
                SortPhotographers();
            }
        }

        private List<Photographer> FilterPhotographers(List<Photographer> allPhotographers)
        {
            switch (FilterBy)
            {
                case "PhotographerName":
                    return allPhotographers
                        .Where(p => p.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                case "Mail":
                    return allPhotographers
                        .Where(p => p.Mail.Contains(FilterCriteria))
                        .ToList();
                case "PhotographerPassword":
                    return allPhotographers
                        .Where(p => p.Password.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                case "PhoneNumber":
                    return allPhotographers
                        .Where(p => p.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                case "CVR":
                    return allPhotographers
                        .Where(p => p.CVR.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                case "All":
                    return allPhotographers
                        .Where(p =>
                            p.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.Mail.Contains(FilterCriteria) ||
                            p.Password.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            p.CVR.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                default:
                    return null;
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
