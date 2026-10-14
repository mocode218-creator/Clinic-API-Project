using Clinic_BusinessLayer.DTOs.Requests.Patients;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Patients
{
    public class CreatePatientValidator : AbstractValidator<CreatePatientRequest>
    {
        public CreatePatientValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Gender)
                .Must(g => g == "Male" || g == "Female")
                .WithMessage("Gender must be 'Male' or 'Female'")
                .When(x => !string.IsNullOrEmpty(x.Gender));

            RuleFor(x => x.BloodType)
                .Must(bt => new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(bt))
                .WithMessage("Invalid blood type")
                .When(x => !string.IsNullOrEmpty(x.BloodType));

            RuleFor(x => x.BirthDate)
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Birth date must be in the past")
                .When(x => x.BirthDate.HasValue);

            RuleFor(x => x.EmergencyContactPhone)
                .MaximumLength(20).WithMessage("Emergency contact phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.EmergencyContactPhone));
        }
    }
}