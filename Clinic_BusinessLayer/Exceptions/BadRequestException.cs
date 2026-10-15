using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, (int)HttpStatusCode.BadRequest, "BAD_REQUEST") { }
    }
}