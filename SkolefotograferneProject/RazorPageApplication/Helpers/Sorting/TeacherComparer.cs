using RazorPageApplication.Enums;
using RazorPageApplication.Models;

namespace RazorPageApplication.Helpers.Sorting
{
    public class TeacherComparer : IComparer<Teacher>
    {
        public TeacherFilterBy FilterBy { get; }
        public TeacherComparer(TeacherFilterBy teacherFilterBy)
        {
            FilterBy = teacherFilterBy;
        }
        public int Compare(Teacher? x, Teacher? y)
        {
            if (x == null || y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            switch (FilterBy)
            {
                case TeacherFilterBy.TeacherID:
                    return x.Id.CompareTo(y.Id);
                case TeacherFilterBy.TeacherName:
                    return x.Name.CompareTo(y.Name);
                case TeacherFilterBy.Mail:
                    return x.Mail.CompareTo(y.Mail);
                case TeacherFilterBy.PhoneNumber:
                    return x.PhoneNumber.CompareTo(y.PhoneNumber);
                default: 
                    return 0;

            }
            
        }
    }
}
