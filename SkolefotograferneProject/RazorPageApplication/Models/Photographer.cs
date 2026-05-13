using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Photographer : IUser
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string CVR { get; set; }
        [Required]
        public UserRole Role { get => UserRole.Photographer; }
        #endregion
        #region Constructors
        public Photographer()
        {

        }
        public Photographer(int id,string name, string mail, string password, string phoneNumber, string cvr)
        {
            Id = id;
            Name = name;
            Mail = mail;
            Password = password;
            PhoneNumber = phoneNumber;
            CVR = cvr;
        } 
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Photographer: \n\tID: {Id}\n\tName: {Name}\n\tMail: {Mail}\n\tPhoneNumber: {PhoneNumber}\n\tCVR: {CVR}";
        } 
        #endregion





    }
}
