using RazorPageApplication.Helpers;

namespace RazorPageApplication.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryExceptionType Type { get; }

        public RepositoryException(RepositoryExceptionType type, string message) : base (message)
        {
            Type = type;
        }

        public override string Message { get { return base.Message; } }
        public string FullMessage { get { return $"Repository exception ({Type}): {this.GetFullMessage()}"; } }
    }

    public enum RepositoryExceptionType
    {
        Create, Read, Update, Delete
    }
}
