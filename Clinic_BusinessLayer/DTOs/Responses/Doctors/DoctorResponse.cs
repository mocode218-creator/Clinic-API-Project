using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Responses.Doctors
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
    }
}