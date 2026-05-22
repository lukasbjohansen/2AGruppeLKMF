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
        /// Instance field that represents the repository for the Student model. 
        /// Used to access the data for the students in the system. 
        /// It is injected into the constructor of the IndexModel class and is used in the OnGet method to retrieve all students from the database and apply filtering and sorting based on user input.
        /// </summary>
        private IRepositoryAsync<Student> _repo;
        #endregion

        #region Properties
        /// <summary>
        /// Property that represents the list of students that will be displayed on the index page.
        /// Used to store the students that are retrieved from the database and filtered and sorted based on user input.
        /// </summary>
        public List<Student> Students { get; set; }
        /// <summary>
        /// Property that represents the filter criteria entered by the user in the search box on the index page.
        /// Used to filter the list of students based on the user's input.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        /// <summary>
        /// Property that represents the filter option selected by the user in the dropdown menu on the index page.
        /// Used to determine which property of the Student model to filter by based on the user's selection.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        /// <summary>
        /// Property that represents the sort option selected by the user in the dropdown menu on the index page.
        /// Used to determine which property of the Student model to sort by based on the user's selection and whether the sorting should be in ascending or descending order.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        /// <summary>
        /// Property that represents whether the sorting should be in descending order or not based on the user's selection in the dropdown menu on the index page.
        /// Used in the SortStudents method to determine the order of sorting when the user selects a sort option.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that is used to create a new instance of the IndexModel class with the repository for the Student model injected as a parameter.
        /// Used to initialize the _repo instance field with the injected repository and is required for the dependency injection to work properly in the Razor Pages application.
        public IndexModel(IRepositoryAsync<Student> studentRepository)
        {
            _repo = studentRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// OnGet method that is called when the index page is accessed via a GET request.
        /// The method retrieves all students from the database using the _repo instance field and applies filtering and sorting based on the user's input in the search box and dropdown menus on the index page.
        /// It first checks if the FilterCriteria property is not null or empty and applies the appropriate filtering based on the value of the FilterBy property.
        /// If the FilterCriteria property is null or empty, it simply assigns all students to the Students property.
        /// If the SortBy property is not null or empty, it calls the SortStudents method to sort the list of students based on the user's selection in the dropdown menu on the index page.
        /// </summary>

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
        /// <summary>
        /// SortStudents method that is called to sort the list of students based on the user's selection in the dropdown menu on the index page.
        /// The method uses a switch statement to determine which property of the Student model to sort by based on the value of the SortBy property and whether the sorting should be in ascending or descending order based on the value of the IsDescending property.
        /// It uses the GenericComparer class from the RazorPageApplication.Helpers.Sorting namespace to perform the sorting based on the selected property and order.
        /// If the SortBy property is "Id", it sorts the students by their Id property. If it is "Name", it sorts by the Name property. If it is "PhotoCode", it sorts by the PhotoCode property. If it is "SchoolClassName", it sorts by the Name property of the SchoolClass associated with each student. If it is "ParentName", it sorts by the Name property of the Parent associated with each student.
        /// If the SortBy property does not match any of the cases in the switch statement, it does not perform any sorting and leaves the list of students in its original order.
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
    

