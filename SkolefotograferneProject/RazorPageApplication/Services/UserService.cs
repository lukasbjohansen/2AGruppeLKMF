using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<Parent> _parentRepo;
        private readonly IRepositoryAsync<Secretary> _secretaryRepo;

        public UserService(IRepositoryAsync<Photographer> photographerRepo,
                           IRepositoryAsync<Parent> parentRepo,
                           IRepositoryAsync<Secretary> secretaryRepo)
        {
            _photographerRepo = photographerRepo;
            _parentRepo = parentRepo;
            _secretaryRepo = secretaryRepo;
        }

        public async Task<List<IUser>> GetAllUsers()
        {
            List<IUser> users = new();
            users.AddRange(await _photographerRepo.GetAllAsync());
            users.AddRange(await _parentRepo.GetAllAsync());
            users.AddRange(await _secretaryRepo.GetAllAsync());
            return users;
        }

        public async Task<IUser> GetUserByUsername(string username)
        {
            List<IUser> users = new();
            users.AddRange(await _photographerRepo.GetAllAsync());
            users.AddRange(await _parentRepo.GetAllAsync());
            users.AddRange(await _secretaryRepo.GetAllAsync());
            return users.FirstOrDefault(u => u.Username.Equals(username));
        }

        public async Task<IUser> VerifyUser(string username, string password)
        {
            foreach (IUser user in await GetAllUsers())
            {
                if (username.Equals(user.Username) && password.Equals(user.Password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
