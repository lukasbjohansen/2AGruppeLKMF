using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class IndexModel : PageModel
    {
        private IRepository<School> _repo;
        public List<School> Schools { get; set; }

        public IndexModel(IRepository<School> schoolRepository)
        {
            _repo = schoolRepository;
        }
        public async Task OnGet()
        {
            Schools = await _repo.GetAllAsync();
        }
    }
}
