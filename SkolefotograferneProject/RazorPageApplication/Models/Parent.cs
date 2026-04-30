using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Parent : IUser
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public Parent()
        {
            
        }

        public Parent(int id, string mail, string password, string name, string phoneNumber, string address, string postalCode)
        {
            Id = id;
            Mail = mail;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            PostalCode = postalCode;
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhoneNumber: {PhoneNumber}\n\tAddress: {Address}\n\tPostalCode: {PostalCode}";
        }
    }
}
