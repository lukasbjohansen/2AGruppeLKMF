using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Reflection;

namespace RazorPageApplication.Pages.Parents
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        public List<Parent> Parents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }

        public async Task OnGet()
        {
            var allParents = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {

                    case "ParentName":
                        Parents = allParents
                            .Where(sc => sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "Mail":
                        Parents = allParents
                            .Where(sc => sc.Mail.ToString().Contains(FilterCriteria))
                            .ToList();
                        break;

                    case "PhoneNumber":
                        Parents = allParents
                            .Where(sc => sc.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "ParentAddress":
                        Parents = allParents
                            .Where(sc => sc.Address.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "PostalCode":
                        Parents = allParents
                            .Where(sc => sc.PostalCode.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "All":
                        allParents.Where(sc =>
                                sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Mail.Contains(FilterCriteria) ||
                                sc.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Address.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }

            }
            else
            {
                Parents = allParents;
            }
        }
    }
}
