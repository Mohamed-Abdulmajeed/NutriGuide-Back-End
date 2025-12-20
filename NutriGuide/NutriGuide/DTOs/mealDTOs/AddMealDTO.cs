using NutriGuide.Custom_Validation;
using NutriGuide.DTOs.ingredientDTO;
using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.mealDTOs
{
    public class AddMealDTO
    {
        [Required]
        [MaxLength(100,ErrorMessage = "Name Must be less than 100")]
        public string Name { get; set; }
        public string Preparation { get; set; }
        [MaxLength(500, ErrorMessage = "Image URL Must be less than 500")]
        public string Image { get; set; }
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public string HealthBenefits { get; set; }
        public string MealType { get; set; }
        public string MealTime { get; set; }
        [RegularExpression("^(Canceled|Passed|Pending)$", ErrorMessage = "Role Must be either 'Pending','Passed','Canceled'")]
        public string Status { get; set; }
        public decimal Budget { get; set; }
        public List<IngredientDTO> ingredientsDTO { get; set; }

        public int? PlanId { get; set; }
        public int? Id { get; set; }

    }
}
