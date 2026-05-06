using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SecretaryRepositoryAsync : IRepository<Secretary>
    {
        public SecretaryRepositoryAsync()
        {
            
        }

        public Task CreateAsync(Secretary item)
        {
            throw new NotImplementedException();
        }

        public async Task CreateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Secretary item)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> FilterUserAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<Secretary> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Secretary>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Secretary item)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }
    }
}
