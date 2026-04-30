using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SecretaryRepository : IUserRepositoryAsync<Secretary>
    {
        public SecretaryRepository()
        {
            
        }

        public Task CreateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task FilterUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(Secretary user)
        {
            throw new NotImplementedException();
        }
    }
}
