using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Meal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Preparation { get; set; }

    public string? Image { get; set; }

    public decimal? Calories { get; set; }

    public decimal? Protein { get; set; }

    public decimal? Carbs { get; set; }

    public decimal? Fat { get; set; }

    public string? HealthBenefits { get; set; }

    public string? MealType { get; set; }

    public string? MealTime { get; set; }

    public string? Status { get; set; }

    public decimal? Budget { get; set; }

    public int? PlanId { get; set; }

    public virtual ICollection<IngredientMeal> IngredientMeals { get; set; } = new List<IngredientMeal>();

    public virtual Plan? Plan { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
