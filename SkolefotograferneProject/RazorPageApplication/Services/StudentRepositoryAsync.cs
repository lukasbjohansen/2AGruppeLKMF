using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class StudentRepositoryAsync : IUserRepositoryAsync<Student>
    {
        public StudentRepositoryAsync()
        {
            
        }

        public async Task CreateUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public async Task FilterUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        Task<List<Student>> IUserRepositoryAsync<Student>.FilterUserAsync(Student user)
        {
            throw new NotImplementedException();
        }
    }
}
