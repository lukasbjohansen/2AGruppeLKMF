using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class TeacherRepository : IUserRepositoryAsync<Teacher>
    {
        public Task CreateUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public Task FilterUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(Teacher user)
        {
            throw new NotImplementedException();
        }
    }
}
