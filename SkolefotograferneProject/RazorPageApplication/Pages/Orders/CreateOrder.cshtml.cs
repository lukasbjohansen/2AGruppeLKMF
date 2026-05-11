using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Orders
{
    public class CreateOrderModel : PageModel
    {
        private IRepositoryAsync<Order> _repo;

        [BindProperty]
        public Order NewOrder { get; set; }

        public CreateOrderModel(IRepositoryAsync<Order> orderRepo)
        {
            _repo = orderRepo;    
        }
        public void OnGet() //MANGLER AT LAVE VIDERE MEN ORDER OG DENS REPO SKAL FIKSE
        {
        }
    }
}
