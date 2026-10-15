using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message)
            : base(message, (int)HttpStatusCode.Unauthorized, "UNAUTHORIZED") { }
    }
}