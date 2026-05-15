using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly ITeacherRepository _repo;

        public List<Teacher> Teachers { get; set; } = new List<Teacher>();

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public TeacherFilterBy FilterBy { get; set; }

        public IndexModel(ITeacherRepository teacherRepository)
        {
            _repo = teacherRepository;
            FilterCriteria = "";
            FilterBy = TeacherFilterBy.All;
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
        }
    }
}