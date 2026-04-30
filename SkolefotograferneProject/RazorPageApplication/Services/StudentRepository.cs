using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class StudentRepository : BaseUserRepository<Student>
    {
        public StudentRepository()
        {
            
        }

        public override void CreateUser(Student user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUser(Student user)
        {
            throw new NotImplementedException();
        }

        public override void FilterUser(Student user)
        {
            throw new NotImplementedException();
        }

        public override List<Student> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Student user)
        {
            throw new NotImplementedException();
        }
    }
}
