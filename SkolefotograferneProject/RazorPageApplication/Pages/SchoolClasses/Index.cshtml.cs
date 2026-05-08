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
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                SchoolClasses = await _repo.FilterAsync(FilterCriteria);
            }
            else
                
            SchoolClasses = await _repo.GetAllAsync();

            }
        
    }
}
