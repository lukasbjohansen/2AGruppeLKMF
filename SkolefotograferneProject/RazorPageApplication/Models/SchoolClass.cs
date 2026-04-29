namespace RazorPageApplication.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public School School { get; set; }
        public Teacher Teacher { get; set; }
        public int Year { get; set; }

        public SchoolClass()
        {
            
        }

        public SchoolClass(int id, string name, School school, Teacher teacher, int year)
        {
            Id = id;
            Name = name;
            School = school;
            Teacher = teacher;
            Year = year;
        }

        public override string ToString()
        {
            return $"School class:\n\tId: {Id}\n\tName: {Name}\n\tSchool: {School}\n\tTeacher: {Teacher}\n\tYear: {Year}";
        }
    }
}
