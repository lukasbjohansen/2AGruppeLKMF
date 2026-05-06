using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class OrderLineRepositoryAsync : IRepository<OrderLine>
	{
		public Task CreateAsync(OrderLine t)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(OrderLine t)
		{
			throw new NotImplementedException();
		}

		public Task<List<OrderLine>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

		public Task<List<OrderLine>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(OrderLine t)
		{
			throw new NotImplementedException();
		}
	}
}
