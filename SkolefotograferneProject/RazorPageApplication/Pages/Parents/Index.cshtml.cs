using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Reflection;

namespace RazorPageApplication.Pages.Parents
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Parent> _repo;

        public List<Parent> Parents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        }
        //public async Task OnGet()
        //{
        //    Parents = await _repo.GetAllAsync();
        //}

        public async Task OnGet()
        {
            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                Parents = await _repo.FilterAsync(FilterCriteria);
            }
            else
                Parents = await _repo.GetAllAsync();
        }
    }
}
