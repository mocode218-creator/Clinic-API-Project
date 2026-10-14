using Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.DoctorSchedules
{
    public class UpdateDoctorScheduleValidator : AbstractValidator<UpdateDoctorScheduleRequest>
    {
        public UpdateDoctorScheduleValidator()
        {
            RuleFor(x => x.DayOfWeek)
                .InclusiveBetween((byte)0, (byte)6)
                .WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday)");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");
        }
    }
}