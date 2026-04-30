using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class PhotographerRepository : BaseUserRepository<Photographer>
    {
        public override void CreateUser(Photographer user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUser(Photographer user)
        {
            throw new NotImplementedException();
        }

        public override void FilterUser(Photographer user)
        {
            throw new NotImplementedException();
        }

        public override List<Photographer> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Photographer user)
        {
            throw new NotImplementedException();
        }
    }
}
