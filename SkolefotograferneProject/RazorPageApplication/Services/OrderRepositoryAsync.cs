using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class OrderRepositoryAsync : IOrderRepositoryAsync
    {
        public async Task CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> FilterOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
