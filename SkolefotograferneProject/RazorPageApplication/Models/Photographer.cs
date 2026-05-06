using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Photographer : IUser
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string CVR { get; set; }
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
