using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Globalization;
using System.Reflection;

namespace RazorPageApplication.Pages.Parents
{
    public class IndexModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<Parent> _repo;
        #endregion

        #region Properties
        public List<Parent> Parents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
        #endregion

        #region Constructors
        public IndexModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        #endregion

        #region Methods
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
            if (!string.IsNullOrEmpty(SortBy))
            {
                SortParents();
            }
        }

        private void SortParents()
        {
            switch (SortBy)
            {
                case "Id":
                    Parents.Sort(new GenericComparer<Parent, int>(p => p.Id, IsDescending));
                    break;
                case "Name":
                    Parents.Sort(new GenericComparer<Parent, string>(p => p.Name, IsDescending));
                    break;
                case "Mail":
                    Parents.Sort(new GenericComparer<Parent, string>(p => p.Mail, IsDescending));
                    break;
                case "PhoneNumber":
                    Parents.Sort(new GenericComparer<Parent, string>(p => p.PhoneNumber, IsDescending));
                    break;
                case "Address":
                    Parents.Sort(new GenericComparer<Parent, string>(p => p.Address, IsDescending));
                    break;
                case "PostalCode":
                    Parents.Sort(new GenericComparer<Parent, string>(p => p.Address, IsDescending));
                    break;
            }
        } 
        #endregion
    }
}
