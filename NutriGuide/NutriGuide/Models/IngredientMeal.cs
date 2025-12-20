using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class IngredientMeal
{
    public int MealId { get; set; }

    public int IngredientId { get; set; }

    public decimal Amount { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Meal Meal { get; set; } = null!;
}
