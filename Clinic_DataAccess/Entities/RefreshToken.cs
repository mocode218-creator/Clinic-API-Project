using System;
using System.Collections.Generic;

namespace Clinic_DataAccess.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }         // ← 🆕 DateTime مش DateTimeOffset

    public DateTime CreatedAt { get; set; }         // ← 🆕

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByToken { get; set; }

    // Navigation
    public virtual User User { get; set; } = null!;
}