using RazorPageApplication.Models;

namespace RazorPageApplication.Interfaces
{
    public interface ISchoolRepositoryAsync
    {
        Task CreateSchoolAsync(School school);
        //Task<School> GetSchool();
        Task<List<School>> GetAllSchoolsAsync();
        Task UpdateSchoolAsync(School school);
        Task DeleteSchoolAsync(School school);
        Task<List<School>> FilterSchoolAsync(string filterCriteria);
    }
}