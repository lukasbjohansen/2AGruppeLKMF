using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class TeacherRepositoryAsync : IUserRepositoryAsync<Teacher>
    {
        public async Task CreateUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>> FilterUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Teacher>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }
    }
}
