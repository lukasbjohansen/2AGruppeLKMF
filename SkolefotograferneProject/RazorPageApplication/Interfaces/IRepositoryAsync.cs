namespace RazorPageApplication.Interfaces
{
    public interface IRepositoryAsync<T> where T : IIdAble
    {
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(T item);
        Task<T?> GetAsync(int id);
        Task<List<T>> FilterAsync(string filterCriteria);
        Task<List<T>> GetAllAsync();
    }
}
