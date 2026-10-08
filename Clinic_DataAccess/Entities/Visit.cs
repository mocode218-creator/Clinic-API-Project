using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class Visit
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? Notes { get; set; }

    public string? Bp { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Weight { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
