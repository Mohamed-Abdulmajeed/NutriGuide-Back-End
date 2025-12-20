using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GoalId { get; set; }

    public int SystemTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public int NumberOfDays { get; set; }

    public int NumberOfMeals { get; set; }

    public decimal? DailyCalories { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual GoalPlan Goal { get; set; } = null!;

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();

    public virtual SystemTypePlan SystemType { get; set; } = null!;
}
