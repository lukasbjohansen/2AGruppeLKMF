using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Teacher : IUser
    {
		#region Properties
        /// <summary>
        /// Unique id. Primarily used by the database. Generated using identity.
        /// </summary>
		public int Id { get; set; }
        /// <summary>
        /// Email address. Must be unique.
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Password. Stored raw for this prototype. Ideally stored as a hash value.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Full name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Phonenumber. Must be unique.
        /// </summary>
        public string PhoneNumber { get; set; }
		#endregion
		#region Constructors
		public Teacher()
        {
            
        }
        public Teacher(int id, string mail, string password, string name, string phoneNumber)
        {
            Id = id;
            Mail = mail;
            Password = password;
            Name = name;
            PhoneNumber = phoneNumber;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return $"Teacher:\n\tId: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}\n\tSchoolclass: {SchoolClass}";
        } 
        #endregion
    }
}
