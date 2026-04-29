namespace RazorPageApplication.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Mail { get; set; }
        string Password { get; set; }
    }
}
