using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Appointments
{
    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int BranchId { get; set; }
        public DateTime StartDate { get; set; }
        public string Type { get; set; } = "Consultation";   // Consultation / FollowUp / Emergency
        public string? Note { get; set; }
    }
}
