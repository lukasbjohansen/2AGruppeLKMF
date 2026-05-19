namespace RazorPageApplication.Interfaces
{
    public interface IUserService
    {
        Task<List<IUser>> GetAllUsers();
        Task<IUser> VerifyUser(string username, string password);
        Task<IUser> GetUserByUsername(string username);
    }
}
