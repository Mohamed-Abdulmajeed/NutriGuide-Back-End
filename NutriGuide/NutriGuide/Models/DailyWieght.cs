using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class DailyWieght
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public decimal Weight { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
