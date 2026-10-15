using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, (int)HttpStatusCode.NotFound, "NOT_FOUND") { }
    }
}