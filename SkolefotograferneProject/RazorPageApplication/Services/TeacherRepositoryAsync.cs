using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class TeacherRepositoryAsync : IRepositoryAsync<Teacher>
    {
        public async Task CreateAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Teacher user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Teacher user)
        {
            throw new NotImplementedException();
        }
    }
}
