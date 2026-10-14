using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Appointments
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("Patient is required");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor is required");

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Branch is required");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment cannot be in the past");

            RuleFor(x => x.StartDate)
                .LessThan(DateTime.UtcNow.AddDays(60))
                .WithMessage("Appointment cannot be booked more than 60 days in advance");

            RuleFor(x => x.StartDate)
                .Must(dt => dt.Minute % 15 == 0 && dt.Second == 0)
                .WithMessage("Appointment must start on a 15-minute slot");

            RuleFor(x => x.Type)
                .Must(t => new[] { "Consultation", "FollowUp", "Emergency" }.Contains(t))
                .WithMessage("Invalid appointment type. Allowed: Consultation, FollowUp, Emergency");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Note cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
}