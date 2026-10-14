using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Appointments
{
    public class UpdateAppointmentRequest
    {
        public DateTime StartDate { get; set; }
        public string Type { get; set; } = "Consultation";
        public string? Note { get; set; }
        public string? Status { get; set; }   // Scheduled / Confirmed / Cancelled / Completed
    }
}