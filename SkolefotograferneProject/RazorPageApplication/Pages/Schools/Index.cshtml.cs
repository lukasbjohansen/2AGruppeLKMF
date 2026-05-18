using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<School> _repo;
        public List<School> Schools { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }

        public IndexModel(IRepositoryAsync<School> schoolRepository)
        {
            _repo = schoolRepository;
        }
        //public async Task OnGet()
        //{
        //    Schools = await _repo.GetAllAsync();
        //}

        public async Task OnGet() //Vi vil fylde vores Members op, til det bruger vi vores metode GetAllMembers fra vores SailClubLibrary
        {
            var allSchools = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {

                    case "SchoolName":
                        Schools = allSchools
                            .Where(sc => sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "SchoolAddress":
                        Schools = allSchools
                            .Where(sc => sc.Address.Contains(FilterCriteria))
                            .ToList();
                        break;

                    case "PostalCode":
                        Schools = allSchools
                            .Where(sc => sc.PostalCode.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "All":
                        Schools = allSchools
                            .Where(sc =>
                                sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Address.ToString().Contains(FilterCriteria) ||
                                sc.PostalCode.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }

            }
            else
            {
                Schools = allSchools;
            }

            if (!string.IsNullOrEmpty(SortBy))
            {
                SortSchools();
            }
        }

        private void SortSchools()
        {
            switch (SortBy)
            {
                case "Id":
                    Schools.Sort(new GenericComparer<School, int>(p => p.Id, IsDescending));
                    break;
                case "Name":
                    Schools.Sort(new GenericComparer<School, string>(p => p.Name, IsDescending));
                    break;
                case "Address":
                    Schools.Sort(new GenericComparer<School, string>(p => p.Address, IsDescending));
                    break;
                case "PostalCode":
                    Schools.Sort(new GenericComparer<School, string>(p => p.PostalCode, IsDescending));
                    break;
            }
        }
    }
}

