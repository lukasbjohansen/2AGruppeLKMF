namespace RazorPageApplication.Interfaces
{
    public interface IUserService
    {
        List<IUser> GetAllUsers();
        IUser VerifyUser(string username, string password);
        IUser GetUserByUsername(string username);
    }
}
