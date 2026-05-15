using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

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

        }


    }
}
