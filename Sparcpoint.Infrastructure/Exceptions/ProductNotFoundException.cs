namespace Sparcpoint.Infrastructure.Exceptions
{
    public class ProductNotFoundException : System.Exception
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string message) : base(message)
        { }
    }
}
