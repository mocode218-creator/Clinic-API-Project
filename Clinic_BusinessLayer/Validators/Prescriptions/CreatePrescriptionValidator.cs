using Clinic_BusinessLayer.DTOs.Requests.Prescriptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Prescriptions
{
    public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionRequest>
    {
        public CreatePrescriptionValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0).WithMessage("Visit is required");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one medication is required");

            RuleForEach(x => x.Items).SetValidator(new CreatePrescriptionItemValidator());
        }
    }

    public class CreatePrescriptionItemValidator : AbstractValidator<DTOs.Requests.PrescriptionItems.CreatePrescriptionItemRequest>
    {
        public CreatePrescriptionItemValidator()
        {
            RuleFor(x => x.MedicationName)
                .NotEmpty().WithMessage("Medication name is required")
                .MaximumLength(150).WithMessage("Medication name cannot exceed 150 characters");

            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage is required")
                .MaximumLength(100).WithMessage("Dosage cannot exceed 100 characters");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required")
                .MaximumLength(100).WithMessage("Frequency cannot exceed 100 characters");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration is required")
                .MaximumLength(100).WithMessage("Duration cannot exceed 100 characters");

            RuleFor(x => x.Instructions)
                .MaximumLength(500).WithMessage("Instructions cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Instructions));
        }
    }
}