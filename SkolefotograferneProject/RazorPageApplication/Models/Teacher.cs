using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Mail { get; set; }
        /// <summary>
        /// Password. Stored raw for this prototype. Ideally stored as a hash value.
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Full name.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Phonenumber. Must be unique.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
		#endregion
		#region Constructors
        /// <summary>
        /// Empty constructor to satisfy razorpages bindproperty.
        /// Initializing each string to avoid compiler-warnings.
        /// </summary>
		public Teacher()
        {
            Mail = "";
            Password = "";
            Name = "";
            PhoneNumber = "";
        }
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="id">Unique id. Primarily used by the database. Generated using identity.</param>
		/// <param name="mail">Email address. Must be unique.</param>
		/// <param name="password">Password. Stored raw for this prototype. Ideally stored as a hash value.</param>
		/// <param name="name">Full name.</param>
		/// <param name="phoneNumber">Phonenumber. Must be unique.</param>
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
		/// <summary>
		/// Gets a tree <see langword="string"/> for the object displayed in an easy-to-read way.
		/// </summary>
		/// <returns>All properties compacted to a <see langword="string"/></returns>
		public override string ToString()
        {
            return $"Teacher:\n\tId: {Id}\n\tMail: {Mail}\n\tName: {Name}\n\tPhonenumber: {PhoneNumber}";
        } 
        #endregion
    }
}
