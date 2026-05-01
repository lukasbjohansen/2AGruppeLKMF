namespace RazorPageApplication.Interfaces
{
    // The DRY way...
    public interface IRepository<T>
    {
        Task Create(T t);
        Task Update(T t);
        Task Delete(T t);
        Task<List<T>> Filter(string filterCriteria);
        Task<List<T>> GetAll();
    }
}
