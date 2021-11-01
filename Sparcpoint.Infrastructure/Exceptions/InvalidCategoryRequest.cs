namespace Sparcpoint.Infrastructure.Exceptions
{
    public  class InvalidCategoryRequest : System.Exception
    {
        public InvalidCategoryRequest()
        {
        }

        public InvalidCategoryRequest(string message) : base(message)
        { }
    }
}
