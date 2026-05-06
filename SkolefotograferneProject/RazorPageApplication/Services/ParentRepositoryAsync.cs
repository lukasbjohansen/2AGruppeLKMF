using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class ParentRepositoryAsync : IRepositoryAsync<Parent>
	{
		public Task CreateAsync(Parent item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Parent item)
		{
			throw new NotImplementedException();
		}

		public Task<List<Parent>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<Parent> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Parent>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Parent item)
		{
			throw new NotImplementedException();
		}
	}
}
