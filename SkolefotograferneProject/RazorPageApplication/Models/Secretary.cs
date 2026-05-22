using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Secretary : IUser
    {
        //Properties til at gemme og sæt værdier ind
        #region Properties
        public int Id { get; set; }
        [Required(ErrorMessage = "Mail er påkrævet")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password er påkrævet")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Telefon nummer er påkrævet")]
        public string PhoneNumber { get; set; }

        public School? School { get; set; }

        public UserRole Role { get => UserRole.Secretary; }
        #endregion


        //Constructor er til når man laver en ny instans og skal enten give den alle argumenterne eller intet
        #region Constructors
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
        #endregion

        //til at kunne printe den enkelte objekts infomation
        #region Methods
        public override string ToString()
        {
            return $"Secretary:\n\tId: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}\n\tSchool: {School}";
        } 
        #endregion
    }
}
