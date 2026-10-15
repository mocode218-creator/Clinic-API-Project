using Clinic_BusinessLayer.DTOs.Requests.Auth;
using FluentValidation;

namespace Clinic_BusinessLayer.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}