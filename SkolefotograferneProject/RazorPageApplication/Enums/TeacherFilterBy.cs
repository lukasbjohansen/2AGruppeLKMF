using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Enums
{
    public enum TeacherFilterBy
    {
        [Display(Name = "Alle")]
        All,

        [Display(Name = "ID")]
        TeacherID,

        [Display(Name = "Navn")]
        TeacherName,

        [Display(Name = "Email")]
        Mail,

        [Display(Name = "Telefonnummer")]
        PhoneNumber
    }
}
