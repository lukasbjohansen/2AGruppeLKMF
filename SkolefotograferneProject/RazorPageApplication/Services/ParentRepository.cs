using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class ParentRepository : BaseUserRepository<Parent>
    {
        public ParentRepository()
        {
            
        }

        public override void CreateUser(Parent parent)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUser(Parent parent)
        {
            throw new NotImplementedException();
        }

        public override void FilterUser(Parent parent)
        {
            throw new NotImplementedException();
        }

        public override List<Parent> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Parent parent)
        {
            throw new NotImplementedException();
        }
    }
}
