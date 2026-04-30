using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class PhotographerRepository : IUserRepositoryAsync<Photographer>
    {
        public Task CreateUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }

        public Task FilterUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photographer>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(Photographer user)
        {
            throw new NotImplementedException();
        }
    }
}
