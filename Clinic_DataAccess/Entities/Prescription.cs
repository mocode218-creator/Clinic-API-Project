using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class Prescription
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();

    public virtual Visit Visit { get; set; } = null!;
}
