using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class AuditLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string TableName { get; set; } = null!;

    public long RecordId { get; set; }

    public string Field { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual User? User { get; set; }
}
