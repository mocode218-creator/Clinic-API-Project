using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, (int)HttpStatusCode.Conflict, "CONFLICT") { }
    }
}