using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Models
{
    public class Student : IIdAble
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public SchoolClass? SchoolClass { get; set; }
        public List<Parent>? Parents { get; set; }
        public string PhotoCode { get; set; }
        #endregion

        #region Constructors
        public Student()
        {

        }

        public Student(int id, string name, SchoolClass? schoolClass, List<Parent>? parents, string photoCode)
        {
            Id = id;
            Name = name;
            SchoolClass = schoolClass;
            Parents = parents;
            PhotoCode = photoCode;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Student:\n\tId: {Id}\n\tName: {Name}\n\tSchoolclass: {SchoolClass}\n\tParents: {Parents}";
        } 
        #endregion
    }
}
