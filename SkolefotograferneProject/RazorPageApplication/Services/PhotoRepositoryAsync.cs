using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class PhotoRepositoryAsync : IRepository<Photo>
	{
		public Task CreateAsync(Photo item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Photo item)
		{
			throw new NotImplementedException();
		}

		public Task<List<Photo>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<Photo> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Photo item)
		{
			throw new NotImplementedException();
		}
	}
}
