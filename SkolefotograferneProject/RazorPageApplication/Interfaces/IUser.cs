using RazorPageApplication.Enums;

namespace RazorPageApplication.Interfaces
{
    public interface IUser : IIdAble
    {
        string Mail { get; set; }
        string Password { get; set; }
        UserRole Role { get; }
    }
}
