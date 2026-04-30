using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class TeacherRepository : BaseUserRepository<Teacher>
    {
        public override void CreateUser(Teacher user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUser(Teacher user)
        {
            throw new NotImplementedException();
        }

        public override void FilterUser(Teacher user)
        {
            throw new NotImplementedException();
        }

        public override List<Teacher> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Teacher user)
        {
            throw new NotImplementedException();
        }
    }
}
