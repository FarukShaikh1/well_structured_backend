namespace FMS_Collection.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized access.") : base(message) { }
    }
}
