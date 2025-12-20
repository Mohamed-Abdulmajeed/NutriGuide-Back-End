using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public string? Type { get; set; }

    public DateTime? ScheduleTime { get; set; }

    public bool? IsRead { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
