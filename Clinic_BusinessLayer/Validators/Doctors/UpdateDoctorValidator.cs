using Clinic_BusinessLayer.DTOs.Requests.Doctors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Doctors
{
    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorRequest>
    {
        public UpdateDoctorValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage("Specialization is required");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("License number is required")
                .MaximumLength(50).WithMessage("License number cannot exceed 50 characters");
        }
    }
}