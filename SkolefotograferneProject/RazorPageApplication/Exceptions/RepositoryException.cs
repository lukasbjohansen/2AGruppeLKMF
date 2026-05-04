namespace RazorPageApplication.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryExceptionType Type { get; }

        public RepositoryException(RepositoryExceptionType type, string message) : base (message)
        {
            Type = type;
        }

        public override string Message { get { return $"{base.Message}"; } }
    }

    public enum RepositoryExceptionType
    {
        Create, Read, Update, Delete
    }
}
