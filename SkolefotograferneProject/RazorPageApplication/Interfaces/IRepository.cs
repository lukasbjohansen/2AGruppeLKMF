namespace RazorPageApplication.Interfaces
{
    // The DRY way...
    public interface IRepository<T>
    {
        Task Create(T t);
        Task Update(T t);
        Task Delete(T t);
        Task<List<T>> Filter(T t);
        Task<List<T>> GetAll();
    }
}
