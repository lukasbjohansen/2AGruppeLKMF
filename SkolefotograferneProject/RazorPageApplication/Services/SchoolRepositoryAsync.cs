using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SchoolRepositoryAsync : ISchoolRepositoryAsync
    {
        public async Task CreateSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public async Task<List<School>> FilterSchoolAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<List<School>> GetAllSchoolsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }
    }
}
