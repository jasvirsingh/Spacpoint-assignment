namespace Sparcpoint.Infrastructure.Exceptions
{
    public class DuplicateProductException : System.Exception
    {
        public DuplicateProductException()
        {
        }

        public DuplicateProductException(string message) : base(message)
        { }
    }
}
