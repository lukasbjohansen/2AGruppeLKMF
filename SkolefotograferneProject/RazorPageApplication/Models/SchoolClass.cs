using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class SchoolClass : IIdAble
    {
        #region Properties
        public int Id { get; set; }

        [Required(ErrorMessage ="Navn er påkrævet")]
        public string Name { get; set; }

        public School? School { get; set; }
        public Teacher? Teacher { get; set; }

        [Required(ErrorMessage = "År er påkrævet")]
        [Range(1900, 3000, ErrorMessage = "År skal være mellem 1900 og 3000")]
        public int? Year { get; set; }
        #endregion

        #region Constructors
        public SchoolClass()
        {

        }

        public SchoolClass(int id, string name, School? school, Teacher? teacher, int year)
        {
            Id = id;
            Name = name;
            School = school;
            Teacher = teacher;
            Year = year;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"School class:\n\tId: {Id}\n\tName: {Name}\n\tSchool: {School}\n\tTeacher: {Teacher}\n\tYear: {Year}";
        } 
        #endregion
    }
}
