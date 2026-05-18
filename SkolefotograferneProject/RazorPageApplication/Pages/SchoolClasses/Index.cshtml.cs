using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Globalization;

namespace RazorPageApplication.Pages.SchoolClasses
{
    public class IndexModel : PageModel
    {

        private IRepositoryAsync<SchoolClass> _repo;
        public List<SchoolClass> SchoolClasses { get; set; }


        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }


        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }


        public IndexModel(IRepositoryAsync<SchoolClass> schoolClassRepository)
        {
            _repo = schoolClassRepository;

        }
        public async Task OnGet()
        {

            var allClasses = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {

                    case "SchoolClassName":
                        SchoolClasses = allClasses
                            .Where(sc => sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "SchoolClassYear":
                        SchoolClasses = allClasses
                            .Where(sc => sc.Year.ToString().Contains(FilterCriteria))
                            .ToList();
                        break;

                    case "SchoolID":
                        SchoolClasses = allClasses
                            .Where(sc => sc.School.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "TeacherID":
                        SchoolClasses = allClasses
                            .Where(sc => sc.Teacher.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "All":
                        SchoolClasses = allClasses
                            .Where(sc =>
                                sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Year.ToString().Contains(FilterCriteria) ||
                                sc.School.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Teacher.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }

            }
            else
            {
                SchoolClasses = allClasses;
            }
                if (!string.IsNullOrEmpty(SortBy))
                {
                    SortSchoolClasses();
                }

        }

        private void SortSchoolClasses()
        {
            switch (SortBy)
            {
                case "Id":
                    SchoolClasses.Sort(new GenericComparer<SchoolClass, int>(p => p.Id, IsDescending));
                    break;
                case "Name":
                    SchoolClasses.Sort(new GenericComparer<SchoolClass, string>(p => p.Name, IsDescending));
                    break;
                case "Year":
                    SchoolClasses.Sort(new GenericComparer<SchoolClass, int>(p => p.Year, IsDescending));
                    break;
                case "SchoolName":
                    SchoolClasses.Sort(new GenericComparer<SchoolClass, string>(p => p.School.Name, IsDescending));
                    break;
                case "TeacherName":
                    SchoolClasses.Sort(new GenericComparer<SchoolClass, string>(p => p.Teacher.Name, IsDescending));
                    break;
            }
        }
    }
}

