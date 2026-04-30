namespace RazorPageApplication.Interfaces
{
    public interface IUserRepositoryAsync<T>
    {
        Task CreateUserAsync(T user);
        Task DeleteUserAsync(T user);
        Task UpdateUserAsync(T user);
        Task FilterUserAsync(T user);
        Task<List<T>> GetAllUsersAsync();
    }
}
