using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.DTOs.Responses.Prescriptions
{
    public class PrescriptionResponse
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionItemResponse> Items { get; set; } = new();
    }
}
