using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Appointments
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentRequest>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment cannot be in the past");

            RuleFor(x => x.StartDate)
                .Must(dt => dt.Minute % 15 == 0 && dt.Second == 0)
                .WithMessage("Appointment must start on a 15-minute slot");

            RuleFor(x => x.Type)
                .Must(t => new[] { "Consultation", "FollowUp", "Emergency" }.Contains(t))
                .WithMessage("Invalid appointment type. Allowed: Consultation, FollowUp, Emergency");

            RuleFor(x => x.Status)
                .Must(s => new[] { "Scheduled", "Confirmed", "Cancelled", "Completed" }.Contains(s))
                .WithMessage("Invalid status. Allowed: Scheduled, Confirmed, Cancelled, Completed")
                .When(x => !string.IsNullOrEmpty(x.Status));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Note cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
}
