using RazorPageApplication.Enums;
using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Photographer : IUser
    {
        #region Properties
        public int Id { get; set; }
        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mail er påkrævet")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Password er krævet")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "VR er påkrævet")]
        public string CVR { get; set; }
        [Required(ErrorMessage ="Brugerrolle er påkrævet")]
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
