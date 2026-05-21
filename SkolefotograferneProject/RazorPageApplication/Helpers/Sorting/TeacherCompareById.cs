using RazorPageApplication.Models;

namespace RazorPageApplication.Helpers.Sorting
{
    public class TeacherCompareById : IComparer<Teacher>
    {
        public int Compare(Teacher? x, Teacher? y)
        {
            if (x == null || y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return x.Id.CompareTo(y.Id);
        }
    }
}
