using RazorPageApplication.Helpers;

namespace RazorPageApplication.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryExceptionType Type { get; }
        public override string Message { get { return base.Message; } }
        public string FullMessage { get { return $"Repository exception ({Type}): {this.GetFullMessage()}"; } }

        public RepositoryException(RepositoryExceptionType type, string message) : base (message)
        {
            Type = type;
        }
    }

    public enum RepositoryExceptionType
    {
        Create, Read, Update, Delete
    }
}
