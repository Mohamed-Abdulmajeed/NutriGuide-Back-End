using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.ingredientDTO
{
    public class IngredientDTO
    {
        [MaxLength(50,ErrorMessage = "Name Must be less than 50 char")]
        public string Name { get; set; }
        [MaxLength(20, ErrorMessage = "Unit Must be less than 20 char")]
        public string Unit { get; set; }
        public decimal Amount { get; set; }


    }
}
