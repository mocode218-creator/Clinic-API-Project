using Clinic_BusinessLayer.DTOs.Requests.Visits;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Visits
{
    public class CreateVisitValidator : AbstractValidator<CreateVisitRequest>
    {
        public CreateVisitValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required");

            RuleFor(x => x.Temperature)
                .InclusiveBetween(30m, 45m)
                .WithMessage("Temperature must be between 30 and 45")
                .When(x => x.Temperature.HasValue);

            RuleFor(x => x.Weight)
                .GreaterThan(0)
                .WithMessage("Weight must be greater than 0")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Symptoms)
                .MaximumLength(1000).WithMessage("Symptoms cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Symptoms));

            RuleFor(x => x.Diagnosis)
                .MaximumLength(1000).WithMessage("Diagnosis cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Diagnosis));
        }
    }
}