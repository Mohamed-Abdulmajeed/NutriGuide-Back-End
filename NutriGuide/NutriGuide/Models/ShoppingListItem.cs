using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class ShoppingListItem
{
    public int Id { get; set; }

    public int? ListId { get; set; }

    public int? IngredientId { get; set; }

    public decimal Amount { get; set; }

    public string Unit { get; set; } = null!;

    public bool? IsBought { get; set; }

    public virtual Ingredient? Ingredient { get; set; }

    public virtual ShoppingList? List { get; set; }
}
