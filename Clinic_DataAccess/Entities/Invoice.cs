using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class Invoice
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Visit Visit { get; set; } = null!;
}
