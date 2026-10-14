using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Doctors
{
    public class CreateDoctorRequest
    {
        // Person
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }

        // Doctor-specific
        public int SpecializationId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
    }
}
