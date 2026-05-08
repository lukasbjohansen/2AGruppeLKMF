using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;


namespace RazorPageApplication.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Order> _repo; 
        public List<Order> Orders { get; set; }

        public IndexModel(IRepositoryAsync<Order> orderRepository)
        {
            _repo = orderRepository;
        }
        public async Task OnGet()
        {
            Orders = await _repo.GetAllAsync();
        }
    }
}
