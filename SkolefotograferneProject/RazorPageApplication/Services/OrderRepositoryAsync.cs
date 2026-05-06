using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class OrderRepositoryAsync : IRepository<Order>
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

        public Task<Order> Get(int id) //async metode
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
