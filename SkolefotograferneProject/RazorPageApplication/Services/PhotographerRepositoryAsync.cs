using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class PhotographerRepositoryAsync : IUserRepositoryAsync<Photographer>
    {
        public async Task CreateUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photographer>> FilterUserAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Photographer>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }
    }
}
