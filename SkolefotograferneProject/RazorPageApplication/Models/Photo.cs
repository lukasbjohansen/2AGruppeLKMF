namespace RazorPageApplication.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
        public Photographer Photographer { get; set; }
        public Student Student { get; set; }

        public Photo(int id, string filePath, DateTime date)
        {
            Id = id;
            FilePath = filePath;
            Date = date;
        }

        public Photo()
        {
            
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tFilePath: {FilePath}\n\tDate: {Date}";
        }
    }
}
