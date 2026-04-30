using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class StudentRepository : IUserRepositoryAsync<Student>
    {
        public StudentRepository()
        {
            
        }

        public Task CreateUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public Task FilterUserAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(Student user)
        {
            throw new NotImplementedException();
        }
    }
}
