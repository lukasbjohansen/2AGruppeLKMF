namespace RazorPageApplication.Interfaces
{
    public interface IUserRepositoryAsync<T>
    {
        Task CreateUserAsync(T user);
        Task DeleteUserAsync(T user);
        Task UpdateUserAsync(T user);
        Task<List<T>> FilterUserAsync(string filterCriteria);
        Task<List<T>> GetAllUsersAsync();
    }
}
