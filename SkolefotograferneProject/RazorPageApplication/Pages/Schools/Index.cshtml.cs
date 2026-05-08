using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Schools
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<School> _repo;
        public List<School> Schools { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(IRepositoryAsync<School> schoolRepository)
        {
            _repo = schoolRepository;
        }
        //public async Task OnGet()
        //{
        //    Schools = await _repo.GetAllAsync();
        //}

        public async Task OnGet() //Vi vil fylde vores Members op, til det bruger vi vores metode GetAllMembers fra vores SailClubLibrary
        {
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                Schools = await _repo.FilterAsync(FilterCriteria);
            }
            else
                Schools = await _repo.GetAllAsync();
        }
    }
}
