using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Student : IIdAble
    {
        #region Properties
        /// <summary>
        /// Id of the student, must be unique and is used to identify the student in the system. 
        /// It is also used as a foreign key in other tables that have a relationship with the student.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Required property that represents the name of the student. 
        /// It is used to identify the student in the system and is also used in the user interface to display the student's name.
        /// </summary>
        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }
        /// <summary>
        /// Property that represents the school class of the student.
        /// It is used to identify the school class that the student belongs to in the system and is also used in the user interface to display the student's school class.
        /// </summary>
        public SchoolClass? SchoolClass { get; set; }
        /// <summary>
        /// Property that represents the parent of the student.
        /// It is used to identify the parent that is associated with the student in the system and is also used in the user interface to display the student's parent.
        /// </summary>
        public Parent? Parent { get; set; }
        /// <summary>
        /// Required property that represents the photo code of the student. 
        /// It is used to identify the student's photo in the system and is also used in the
        /// StringLength attribute to ensure that the photo code is not too long and does not exceed the maximum length of 15 characters.
        /// </summary>
        [Required(ErrorMessage = "Fotokode er påkrævet")]
        [StringLength(15, ErrorMessage = "Fotokoden må maks være 15 tegn langt")]
        public string PhotoCode { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructor that is used to create a new instance of the Student class without setting any properties.
        /// This constructor is required by the Entity Framework when it creates instances of the Student class when retrieving data from the database.
        /// </summary>
        public Student()
        {

        }
        /// <summary>
        /// Constructor that is used to create a new instance of the Student class with all properties set.
        /// </summary>

        public Student(int id, string name, SchoolClass? schoolClass, Parent? parent, string photoCode)
        {
            Id = id;
            Name = name;
            SchoolClass = schoolClass;
            Parent = parent;
            PhotoCode = photoCode;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a string representation of the Student object.
        /// </summary>
        public override string ToString()
        {
            return $"Student:\n\tId: {Id}\n\tName: {Name}\n\tSchoolclass: {SchoolClass}\n\tParent: {Parent.Name}";
        } 
        #endregion
    }
}
