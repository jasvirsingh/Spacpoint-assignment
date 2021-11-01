namespace Sparcpoint.Infrastructure.Exceptions
{
    public class DuplicateCategoryException : System.Exception
    {
        public DuplicateCategoryException()
        {
        }

        public DuplicateCategoryException(string message) : base(message)
        { }
    }
}