using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class PhotographerRepositoryAsync : IRepository<Photographer>
	{
		public Task CreateAsync(Photographer item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Photographer item)
		{
			throw new NotImplementedException();
		}

		public Task<List<Photographer>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<Photographer> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photographer>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Photographer item)
		{
			throw new NotImplementedException();
		}
	}
}
