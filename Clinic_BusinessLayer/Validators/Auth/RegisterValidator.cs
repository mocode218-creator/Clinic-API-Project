using Clinic_BusinessLayer.DTOs.Requests.Auth;
using FluentValidation;

namespace Clinic_BusinessLayer.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Role is required");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}