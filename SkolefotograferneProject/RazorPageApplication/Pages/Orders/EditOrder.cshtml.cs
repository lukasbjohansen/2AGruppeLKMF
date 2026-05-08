using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Orders
{
    public class EditOrderModel : PageModel
    {
        private readonly IRepositoryAsync<Order> _repo;

        [BindProperty]
        public Order? OrderToUpdate { get; set; }

        public EditOrderModel(IRepositoryAsync<Order> orderRepo)
        {
            _repo = orderRepo;
        }
        public async Task OnGet(int id)
        {
            OrderToUpdate = await _repo.GetAsync(id);
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.UpdateAsync(OrderToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}
