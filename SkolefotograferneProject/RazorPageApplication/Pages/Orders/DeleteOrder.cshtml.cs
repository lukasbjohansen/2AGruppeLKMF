using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Orders
{
    public class DeleteOrderModel : PageModel
    {
        private readonly IRepositoryAsync<Order> _repo;
        public Order DeleteOrder { get; set; }

        public DeleteOrderModel(IRepositoryAsync<Order> repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            DeleteOrder = await _repo.GetAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            DeleteOrder = await _repo.GetAsync(id);
            await _repo.DeleteAsync(DeleteOrder);
            return RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
