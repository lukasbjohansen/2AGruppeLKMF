using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Teacher : IUser
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public Teacher()
        {
            
        }
        public Teacher(int id, string mail, string password, string name, string phoneNumber, SchoolClass schoolClass)
        {
            Id = id;
            Mail = mail;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
            SchoolClass = schoolClass;
        }

        public override string ToString()
        {
            return $"Teacher:\n\tId: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}\n\tSchoolclass: {SchoolClass}";
        }
    }

    
}
