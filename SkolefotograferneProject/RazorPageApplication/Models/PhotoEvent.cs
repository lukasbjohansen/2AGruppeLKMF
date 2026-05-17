using RazorPageApplication.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

namespace RazorPageApplication.Models
{
    public class PhotoEvent : IIdAble
    {
        #region Properties
        public int Id { get; set; }

        [Required(ErrorMessage ="Starttidspunkt er påkrævet")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Sluttidspunkt er påkrævet")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Lokation er påkrævet")]
        public string Location { get; set; }

        public Photographer? Photographer { get; set; }

        public SchoolClass? SchoolClass { get; set; } 
        #endregion

        #region Constructors
        public PhotoEvent()
        {

        }

        public PhotoEvent(int id, DateTime startTime, DateTime endTime, string location, Photographer? photographer, SchoolClass? schoolClass)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Location = location;
            Photographer = photographer;
            SchoolClass = schoolClass;
        } 
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"PhotoEvent: \n\tID: {Id}\n\tStartTime: {StartTime}\n\tEndTime: {EndTime}\n\tLocation: {Location}\n\tPhotographer: {Photographer}\n\tSchoolClass: {SchoolClass}";
        } 
        #endregion
    }
}
