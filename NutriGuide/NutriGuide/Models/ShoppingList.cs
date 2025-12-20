using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class ShoppingList
{
    public int Id { get; set; }

    public int? PlanId { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? GeneratedDate { get; set; }

    public virtual Plan? Plan { get; set; }

    public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}
