using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Services
{
    public abstract class BaseUserRepository<T>
    {
        protected BaseUserRepository()
        {
            
        }

        public abstract void CreateUser(T user);
        public abstract void DeleteUser(T user);
        public abstract void UpdateUser(T user);
        public abstract void FilterUser(T user);
        public abstract List<T> GetAllUsers();
    }
}
