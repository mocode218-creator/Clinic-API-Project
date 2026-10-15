using Clinic_BusinessLayer.DTOs.Requests.Auth;
using FluentValidation;

namespace Clinic_BusinessLayer.Validators.Auth
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");
        }
    }
}