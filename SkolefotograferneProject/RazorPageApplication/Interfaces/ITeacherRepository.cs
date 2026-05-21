using RazorPageApplication.Enums;
using RazorPageApplication.Models;

namespace RazorPageApplication.Interfaces
{
    public interface ITeacherRepository : IRepositoryAsync<Teacher>
    {
        Task<List<Teacher>> FilterAsync(string filterCriteria, TeacherFilterBy filterBy);
        Task<List<Teacher>> Sort();
    }
}
