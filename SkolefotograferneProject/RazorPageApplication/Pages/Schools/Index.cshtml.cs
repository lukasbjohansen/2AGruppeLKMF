using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class IndexModel : PageModel
    {
        private ISchoolRepositoryAsync _repo;
        public List<School> Schools { get; set; }

        public IndexModel(ISchoolRepositoryAsync schoolRepository)
        {
            _repo = schoolRepository;
        }
        public async Task OnGet()
        {
            Schools = await _repo.GetAllSchoolsAsync();
        }
    }
}
