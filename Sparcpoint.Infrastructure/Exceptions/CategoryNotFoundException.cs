namespace Sparcpoint.Infrastructure.Exceptions
{
    public class CategoryNotFoundException : System.Exception
    {
        public CategoryNotFoundException()
        {
        }

        public CategoryNotFoundException(string message) : base(message)
        { }
    }
}
