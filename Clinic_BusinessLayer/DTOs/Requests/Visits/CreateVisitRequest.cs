using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Visits
{
    public class CreateVisitRequest
    {
        public int AppointmentId { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public string? Bp { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Weight { get; set; }
    }
}
