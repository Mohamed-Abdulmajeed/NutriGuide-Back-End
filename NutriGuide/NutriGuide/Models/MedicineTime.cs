using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class MedicineTime
{
    public int Id { get; set; }

    public int MedicineId { get; set; }

    public TimeOnly TakeTime { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;
}
