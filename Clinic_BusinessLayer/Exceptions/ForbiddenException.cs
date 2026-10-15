using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message)
            : base(message, (int)HttpStatusCode.Forbidden, "FORBIDDEN") { }
    }
}