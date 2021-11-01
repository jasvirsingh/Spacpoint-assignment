namespace Sparcpoint.Infrastructure.Exceptions
{
    public class InvalidProductRequest : System.Exception
    {
        public InvalidProductRequest()
        {
        }

        public InvalidProductRequest(string message) : base(message)
        { }
    }
}
