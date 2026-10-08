using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class Patient
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }

    public string? BloodType { get; set; }

    public string? Allergies { get; set; }

    public string? ChronicConditions { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Person Person { get; set; } = null!;
}
