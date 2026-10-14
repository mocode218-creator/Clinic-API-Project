using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.PrescriptionItems
{
    public class CreatePrescriptionItemRequest
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string? Instructions { get; set; }
    }
}