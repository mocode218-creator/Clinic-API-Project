using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PersonId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
