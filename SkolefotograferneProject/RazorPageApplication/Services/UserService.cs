using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Services
{
    public class UserService : IUserService
    {
        public List<IUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IUser GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IUser VerifyUser(string username, string password)
        {
            foreach(IUser user in GetAllUsers())
            {
                if(username.Equals(user.Mail) && password.Equals(user.Password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
