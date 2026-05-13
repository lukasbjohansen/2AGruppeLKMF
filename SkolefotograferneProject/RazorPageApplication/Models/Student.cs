using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Student : IIdAble
    {
        #region Properties
        public int Id { get; set; }

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string Name { get; set; }
 
        public SchoolClass? SchoolClass { get; set; }
      
        public Parent? Parent { get; set; }

        [Required(ErrorMessage = "Fotokode er påkrævet")]
        [StringLength(15, ErrorMessage = "Fotokoden må maks være 15 tegn langt")]
        public string PhotoCode { get; set; }

        #endregion

        #region Constructors
        public Student()
        {

        }

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
        public override string ToString()
        {
            return $"Student:\n\tId: {Id}\n\tName: {Name}\n\tSchoolclass: {SchoolClass}\n\tParent: {Parent.Name}";
        } 
        #endregion
    }
}
