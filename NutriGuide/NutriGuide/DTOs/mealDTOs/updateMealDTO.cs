using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.mealDTOs
{
    public class updateMealDTO
    {
        [Required]
        public int MealId { get; set; }
        [RegularExpression("^(Canceled|Passed|Pending)$", ErrorMessage = "Role Must be either 'Pending','Passed','Canceled'")]
        public string? NewStatus { get; set; }
    }
}
