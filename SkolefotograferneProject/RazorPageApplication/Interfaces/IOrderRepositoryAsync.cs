using RazorPageApplication.Models;

namespace RazorPageApplication.Interfaces
{
    public interface IOrderRepositoryAsync
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
        Task<List<Order>> FilterOrder(string filterCriteria);
        Task<List<Order>> GetAllOrders();
    }
}