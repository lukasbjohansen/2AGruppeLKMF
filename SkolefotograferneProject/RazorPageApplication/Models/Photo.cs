using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Models
{
    public class Photo : IIdAble
    {
        #region Properties
        public int Id { get; set; }
        [Required(ErrorMessage = "Filnavn er påkrævet")]
        public string FilePath { get; set; }
        [Required(ErrorMessage = "Gyldig dato er påkrævet")]
        public DateTime Date { get; set; }
        public Photographer? Photographer { get; set; }
        public Student? Student { get; set; }
        #endregion

        #region Constructors
        public Photo(int id, string filePath, DateTime date, Photographer? photographer, Student? student)
        {
            Id = id;
            FilePath = filePath;
            Date = date;
            Photographer = photographer;
            Student = student;
        }

        public Photo()
        {

        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Id: {Id}\n\tFilePath: {FilePath}\n\tDate: {Date}\n\tPhotographer: {Photographer.Name}\n\tStudent: {Student.Name}";
        }
        #endregion
    }
}
