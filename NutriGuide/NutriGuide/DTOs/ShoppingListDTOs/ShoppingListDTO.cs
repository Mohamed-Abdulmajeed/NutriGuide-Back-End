namespace NutriGuide.DTOs.ShoppingListDTOs
{
    public class ShoppingListDTO
    {

        public int PlanId { get; set; }

        public bool IsCompleted { get; set; }
        
        public DateTime GeneratedDate { get; set; }
        public List<ShoppingListItemDTO> Items { get; set; }
      
    }

}
