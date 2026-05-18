namespace RazorPageApplication.Helpers.Sorting
{
    public class GenericComparer<T, TKey> : IComparer<T> where TKey : IComparable
    {
        private readonly Func<T, TKey> _selector;
        private readonly bool _descending;

        public GenericComparer(Func<T, TKey> selector, bool descending = false)
        {
            _selector = selector;
            _descending = descending;
        }

        public int Compare(T? x, T? y)  
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            int res = _selector(x).CompareTo(_selector(y));
            return _descending ? -res : res;
        }
    }
}
