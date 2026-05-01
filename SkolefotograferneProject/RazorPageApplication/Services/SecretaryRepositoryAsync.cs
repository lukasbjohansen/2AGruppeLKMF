using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SecretaryRepositoryAsync : IUserRepositoryAsync<Secretary>
    {
        public SecretaryRepositoryAsync()
        {
            
        }

        public async Task CreateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> FilterUserAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Secretary>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }
    }
}
