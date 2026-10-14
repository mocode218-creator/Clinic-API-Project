using Clinic_BusinessLayer.DTOs.Requests.Branches;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Branches
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchRequest>
    {
        public UpdateBranchValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.WorkingHours)
                .MaximumLength(100).WithMessage("Working hours cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.WorkingHours));
        }
    }
}