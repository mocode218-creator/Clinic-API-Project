using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class Doctor
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int SpecializationId { get; set; }

    public string LicenseNumber { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    public virtual Person Person { get; set; } = null!;

    public virtual Specialization Specialization { get; set; } = null!;
}
