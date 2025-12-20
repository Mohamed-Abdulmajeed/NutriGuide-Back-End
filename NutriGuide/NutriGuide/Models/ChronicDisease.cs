using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class ChronicDisease
{
    public int Id { get; set; }

    public string DiseaseName { get; set; } = null!;

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
