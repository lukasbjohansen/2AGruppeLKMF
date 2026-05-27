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
        /// <summary>
        /// _studentRepo instance field that represents the repository for the Student model.
        /// </summary>
        private IRepositoryAsync<Student> _studentRepo;
        #endregion

        #region Properties
        /// <summary>
        /// Students property that represents the list of students that will be displayed on the index page.
        /// </summary>
        public List<Student> Students { get; set; }

        /// <summary>
        /// FilterCriteria property that represents the search term entered by the user in the search box on the index page.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        /// <summary>
        /// FilterBy property that represents the filter option selected by the user in the dropdown menu on the index page.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        /// <summary>
        /// SortBy property that represents the sort option selected by the user in the dropdown menu on the index page.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        /// <summary>
        /// IsDescending property that represents whether the sorting should be in descending order or not based on the user's selection in the dropdown menu on the index page.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        ///IndexModel constructor that takes an IRepositoryAsync<Student> parameter and assigns it to the _studentRepo instance field. This allows the IndexModel class to access the methods of the repository to retrieve and manipulate student data from the database when the index page is accessed.
        /// </summary>
        public IndexModel(IRepositoryAsync<Student> studentRepo)
        {
            _studentRepo = studentRepo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// OnGet method that is called when the index page is accessed via a GET request.
        /// </summary>

        public async Task OnGet()
        {
            var allStudents = await _studentRepo.GetAllAsync();

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

        /// <summary>
        /// SortStudents method that is called to sort the list of students based on the user's selection in the dropdown menu on the index page.
        /// </summary>

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
    

