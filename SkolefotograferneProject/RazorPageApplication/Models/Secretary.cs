using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Secretary : IUser
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public School School { get; set; }

        public Secretary()
        {

        }
        public Secretary(int id, string mail, string password, string name, string phoneNumber, School school)
        {
            Id = id;
            Mail = mail;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
            School = school;
        }

        public override string ToString()
        {
            return $"Secretary:\n\tId: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}\n\tSchool: {School}";
        }
    }
}
