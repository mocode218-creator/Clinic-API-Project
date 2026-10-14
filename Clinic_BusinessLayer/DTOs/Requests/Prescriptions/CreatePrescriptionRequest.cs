using Clinic_BusinessLayer.DTOs.Requests.PrescriptionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Requests.Prescriptions
{
    public class CreatePrescriptionRequest
    {
        public int VisitId { get; set; }
        public string? Notes { get; set; }
        public List<CreatePrescriptionItemRequest> Items { get; set; } = new();
    }
}
