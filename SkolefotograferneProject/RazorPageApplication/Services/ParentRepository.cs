using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class ParentRepository : IUserRepositoryAsync<Parent>
    {
        public ParentRepository()
        {
            
        }

        public Task CreateUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }

        public Task FilterUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Parent>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }
    }
}
