namespace RazorPageApplication.Models
{
    public class School
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public List<SchoolClass> SchoolClasses { get; set; }
        public Secretary? Secretary { get; set; }
        #endregion

        #region Constructors
        public School()
        {
            SchoolClasses = new List<SchoolClass>();
           
        }

        public School(int id, string name, string address, string postalCode, List<SchoolClass> schoolClasses, Secretary secretary)
        {
            Id = id;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            SchoolClasses = schoolClasses;
            Secretary = secretary;
        }

        public School(int id, string name, string address, string postalCode)
        {
            Id = id;
            Name = name;
            Address = address;
            PostalCode = postalCode;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"School:\n\tId: {Id}\n\tName: {Name}\n\tAddress: {Address}\n\tPostal code: {PostalCode}\n\tSecretary: {Secretary}";
        } 
        #endregion
    }
}
