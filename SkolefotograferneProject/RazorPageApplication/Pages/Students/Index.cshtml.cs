using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Students
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Student> _repo;
        public List<Student> Students { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }
        public IndexModel(IRepositoryAsync<Student> studentRepository)
        {
            _repo = studentRepository;
        }
        public async Task OnGet()
        {
            var allStudents = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {

                    case "StudentName":
                        Students = allStudents
                            .Where(sc => sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "PhotoCode":
                        Students = allStudents
                            .Where(sc => sc.PhotoCode.ToString().Contains(FilterCriteria))
                            .ToList();
                        break;

                    case "SchoolClassID":
                        Students = allStudents
                            .Where(sc => sc.SchoolClass.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "ParentID":
                        Students = allStudents
                            .Where(sc => sc.Parent.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "All":
                        Students = allStudents
                            .Where(sc =>
                                sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.PhotoCode.ToString().Contains(FilterCriteria) ||
                                sc.SchoolClass.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Parent.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }

            }
            else
            {
                Students = allStudents;
            }
        }
    }
}
