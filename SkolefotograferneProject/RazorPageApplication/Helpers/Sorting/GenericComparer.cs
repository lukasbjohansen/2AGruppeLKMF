namespace RazorPageApplication.Helpers.Sorting
{
    /// <summary>
    /// A generic IComparer implementation.
    /// Utilizes a Func key selector delegate to encapsulate the data we wish to compare.
    /// </summary>
    public class GenericComparer<T, TKey> : IComparer<T> where TKey : IComparable
    {
        private readonly Func<T, TKey> _selector;
        private readonly bool _descending;

        /// <summary>
        /// Initializes a new instance of the GenericComparer class.
        /// Takes two parameters, a delegate 'selector', where we specify the object type we wish to compare from,
        /// and the key data type we wish to compare, as well as a bool 'descending' to specify the order of the compared output.
        /// </summary>
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
            return _descending
                ? _selector(y).CompareTo(_selector(x))
                : _selector(x).CompareTo(_selector(y));
        }
    }
}
