using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Students
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Student> _repo;
        public List<Student> Students { get; set; }


        public IndexModel(IRepositoryAsync<Student> studentRepository)
        {
            _repo = studentRepository;
        }
        public async Task OnGet()
        {
            Students = await _repo.GetAllAsync();
        }
    }
}
