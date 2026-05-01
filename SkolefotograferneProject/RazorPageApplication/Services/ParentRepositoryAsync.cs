using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class ParentRepositoryAsync : IUserRepositoryAsync<Parent>
    {
        public ParentRepositoryAsync()
        {
            
        }

        public async Task CreateUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Parent>> FilterUserAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Parent>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Parent user)
        {
            throw new NotImplementedException();
        }
    }
}
