using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SecretaryRepository : BaseUserRepository<Secretary>
    {
        public SecretaryRepository()
        {
            
        }

        public override void CreateUser(Secretary user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUser(Secretary user)
        {
            throw new NotImplementedException();
        }

        public override void FilterUser(Secretary user)
        {
            throw new NotImplementedException();
        }

        public override List<Secretary> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Secretary user)
        {
            throw new NotImplementedException();
        }
    }
}
