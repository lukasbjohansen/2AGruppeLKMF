using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class PhotoEventRepositoryAsync : IRepository<PhotoEvent>
	{
		public Task CreateAsync(PhotoEvent item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(PhotoEvent item)
		{
			throw new NotImplementedException();
		}

		public Task<List<PhotoEvent>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

		public Task<List<PhotoEvent>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(PhotoEvent item)
		{
			throw new NotImplementedException();
		}
	}
}
