using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Globalization;

namespace RazorPageApplication.Pages.Students
{
    public class IndexModel : PageModel
    {
        #region Instance fields
        private IRepositoryAsync<Student> _repo;
        #endregion

        #region Properties
        public List<Student> Students { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }


        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }
        #endregion

        #region Constructors
        public IndexModel(IRepositoryAsync<Student> studentRepository)
        {
            _repo = studentRepository;
        }
        #endregion

        #region Methods
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
            if (!string.IsNullOrEmpty(SortBy))
            {
                SortStudents();
            }

        }

        private void SortStudents()
        {
            switch (SortBy)
            {
                case "Id":
                    Students.Sort(new GenericComparer<Student, int>(p => p.Id, IsDescending));
                    break;
                case "Name":
                    Students.Sort(new GenericComparer<Student, string>(p => p.Name, IsDescending));
                    break;
                case "PhotoCode":
                    Students.Sort(new GenericComparer<Student, string>(p => p.PhotoCode, IsDescending));
                    break;
                case "SchoolClassName":
                    Students.Sort(new GenericComparer<Student, string>(p => p.SchoolClass.Name, IsDescending));
                    break;
                case "ParentName":
                    Students.Sort(new GenericComparer<Student, string>(p => p.Parent.Name, IsDescending));
                    break;
            }
        }
        #endregion
    } 
}
    

