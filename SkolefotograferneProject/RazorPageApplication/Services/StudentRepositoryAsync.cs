using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class StudentRepositoryAsync : IRepository<Student>
    {
        public StudentRepositoryAsync()
        {
            
        }

        public async Task CreateAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Student user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Student user)
        {
            throw new NotImplementedException();
        }
    }
}
