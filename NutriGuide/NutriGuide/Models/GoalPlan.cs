using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class GoalPlan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
}
