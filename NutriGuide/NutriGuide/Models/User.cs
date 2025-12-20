using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool? IsVerified { get; set; }

    public string? VerificationCode { get; set; }

    public DateTime? VerificationCodeExpiry { get; set; }

    public string? ResetPasswordCode { get; set; }

    public DateTime? ResetPasswordExpiry { get; set; }

    public int? ResetAttempts { get; set; }

    public virtual Customer? Customer { get; set; }
}
