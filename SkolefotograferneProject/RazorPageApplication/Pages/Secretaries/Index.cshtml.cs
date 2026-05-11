using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Threading.Tasks;

namespace RazorPageApplication.Pages.Secretaries
{
    public class IndexModel : PageModel
    {
        IRepositoryAsync<Secretary> _repo;
        public List<Secretary> Secretaries { get; set; }
        public IndexModel(IRepositoryAsync<Secretary> secretaryRepo)
        {
            _repo = secretaryRepo;
        }
        public async Task OnGet()
        {
            Secretaries = await _repo.GetAllAsync();
        }
    }
}
