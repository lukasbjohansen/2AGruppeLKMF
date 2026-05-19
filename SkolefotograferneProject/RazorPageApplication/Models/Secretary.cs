using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Secretary : IUser
    {
        #region Properties
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public School? School { get; set; }
        public UserRole Role { get => UserRole.Secretary; }
        #endregion

        #region Constructors
        public Secretary()
        {

        }
        public Secretary(int id, string mail, string password, string name, string phoneNumber, School school)
        {
            Id = id;
            Username = mail;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
            School = school;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Secretary:\n\tId: {Id}\n\tMail: {Username}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}\n\tSchool: {School}";
        } 
        #endregion
    }
}
