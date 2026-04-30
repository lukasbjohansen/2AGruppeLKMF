namespace RazorPageApplication.Models
{
    public class Photo
    {
        #region Properties
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
        public Photographer Photographer { get; set; }
        public Student Student { get; set; }
        #endregion

        #region Constructors
        public Photo(int id, string filePath, DateTime date)
        {
            Id = id;
            FilePath = filePath;
            Date = date;
        }
        #endregion

        public Photo()
        {

        }

        #region Methods
        public override string ToString()
        {
            return $"Id: {Id}\n\tFilePath: {FilePath}\n\tDate: {Date}";
        }
        #endregion
    }
}
