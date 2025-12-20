namespace NutriGuide.DTOs.ShoppingListDTOs
{
    public class ShoppingListItemDTO
    {
            public string IngredientName { get; set; }
            public decimal Amount { get; set; }
            public string Unit { get; set; }
            public bool IsBought { get; set; }
        
    }
}
