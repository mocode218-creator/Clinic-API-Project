using Clinic_BusinessLayer.DTOs.Requests.Specializations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Specializations
{
    public class UpdateSpecializationValidator : AbstractValidator<UpdateSpecializationRequest>
    {
        public UpdateSpecializationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        }
    }
}

