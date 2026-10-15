using System.Net;

namespace Clinic_BusinessLayer.Exceptions
{
    public class ValidationException : AppException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("Validation failed", (int)HttpStatusCode.BadRequest, "VALIDATION_ERROR")
        {
            Errors = errors;
        }

        public ValidationException(string message)
            : base(message, (int)HttpStatusCode.BadRequest, "VALIDATION_ERROR")
        {
            Errors = new[] { message };
        }
    }
}