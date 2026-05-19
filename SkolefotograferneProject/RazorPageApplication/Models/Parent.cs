using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Parent : IUser
    {
        #region Propeties
        public int Id { get; set; }
        [Required(ErrorMessage = "Mail er påkrævet")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password er påkrævet")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        [StringLength(11, ErrorMessage = "Telefonnummeret må være max 11 karakterer")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Adresse er påkrævet")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Gyldigt postnummer er påkrævet")]
        public string PostalCode { get; set; }
        public UserRole Role { get => UserRole.Parent; }
        #endregion

        #region Constructors
        public Parent()
        {

        }

        public Parent(int id, string username, string password, string name, string phoneNumber, string address, string postalCode)
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            PostalCode = postalCode;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Id: {Id}\n\tMail: {Username}\n\tName: {Name}\n\tPhoneNumber: {PhoneNumber}\n\tAddress: {Address}\n\tPostalCode: {PostalCode}";
        }
        #endregion
    }
}
