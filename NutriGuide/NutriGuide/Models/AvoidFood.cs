using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class AvoidFood
{
    public int Id { get; set; }

    public string FoodName { get; set; } = null!;

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
