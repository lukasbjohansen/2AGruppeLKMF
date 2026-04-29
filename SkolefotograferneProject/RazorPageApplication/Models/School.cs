namespace RazorPageApplication.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public List<SchoolClass> SchoolClasses { get; set; }
        public List<Teacher> Teachers { get; set; }
        public Secretary Secretary { get;set; }

        public School()
        {
            
        }

        public School(int id, string name, string address, string postalCode, List<SchoolClass> schoolClasses, List<Teacher> teachers, Secretary secretary)
        {
            Id = id;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            SchoolClasses = schoolClasses;
            Teachers = teachers;
            Secretary = secretary;
        }

        public override string ToString()
        {
            return $"School:\n\tId: {Id}\n\tName: {Name}\n\tAddress: {Address}\n\tPostal code: {PostalCode}\n\tSecretary: {Secretary}";
        }
    }
}
