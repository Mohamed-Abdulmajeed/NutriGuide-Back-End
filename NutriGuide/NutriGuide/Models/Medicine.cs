using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string MedicineName { get; set; } = null!;

    public string Option { get; set; } = null!;

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<MedicineTime> MedicineTimes { get; set; } = new List<MedicineTime>();
}
