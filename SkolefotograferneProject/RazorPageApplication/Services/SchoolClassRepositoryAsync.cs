using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
	public class SchoolClassRepositoryAsync : IRepository<SchoolClass>
	{
		public Task CreateAsync(SchoolClass item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(SchoolClass item)
		{
			throw new NotImplementedException();
		}

		public Task<List<SchoolClass>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<SchoolClass> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SchoolClass>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(SchoolClass item)
		{
			throw new NotImplementedException();
		}
	}
}
