using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class OrderRepositoryAsync : IRepositoryAsync<Order>
	{
		public Task CreateAsync(Order item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Order item)
		{
			throw new NotImplementedException();
		}

		public Task<List<Order>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<Order> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Order item)
		{
			throw new NotImplementedException();
		}
	}
}
