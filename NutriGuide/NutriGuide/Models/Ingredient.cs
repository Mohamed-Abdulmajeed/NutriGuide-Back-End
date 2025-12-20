using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public virtual ICollection<IngredientMeal> IngredientMeals { get; set; } = new List<IngredientMeal>();

    public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}
