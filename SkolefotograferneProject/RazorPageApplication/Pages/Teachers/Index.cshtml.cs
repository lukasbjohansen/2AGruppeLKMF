using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Enums;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Globalization;

namespace RazorPageApplication.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly ITeacherRepository _repo;
        private bool _isDescending;

        public List<Teacher> Teachers { get; set; } = new List<Teacher>();

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public TeacherFilterBy FilterBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public string LastSortBy { get; set; }

        public IndexModel(ITeacherRepository teacherRepository)
        {
            _repo = teacherRepository;
            _isDescending = false;
        }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(FilterCriteria))
            {
                Teachers = await _repo.GetAllAsync();
            }
            else
            {
                Teachers = await _repo.FilterAsync(FilterCriteria, FilterBy);
            }
            if (!string.IsNullOrEmpty(SortBy)) SortTeachers();
        }
        private void SortTeachers()
        {
            if (LastSortBy == SortBy) _isDescending = !_isDescending;
            else _isDescending = false;
            LastSortBy = SortBy;
            switch (SortBy)
            {
                case "Id":
                    Teachers.Sort(new GenericComparer<Teacher, int>(p => p.Id, _isDescending));
                    break;
                case "Name":
                    Teachers.Sort(new GenericComparer<Teacher, string>(p => p.Name, _isDescending));
                    break;
                case "Mail":
                    Teachers.Sort(new GenericComparer<Teacher, string>(p => p.Mail, _isDescending));
                    break;
                case "PhoneNumber":
                    Teachers.Sort(new GenericComparer<Teacher, string>(p => p.PhoneNumber, _isDescending));
                    break;
            }
        }
    }
}