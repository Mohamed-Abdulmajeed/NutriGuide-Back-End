using NutriGuide.Custom_Validation;
using NutriGuide.DTOs.mealDTOs;
using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.planDTOs
{
    public class AddPlanDTO
    {
        [Required]
        [MaxLength(100,ErrorMessage = "Name Must be less than 100")]
        public string Name { get; set; }
        public string Goal { get; set; }
        public string SystemType { get; set; }
        public int NumberOfDays { get; set; }

        [RegularExpression("^(1|2|3|4)$", ErrorMessage = "Number of Meals Must be between 1 to 4")]
        public int NumberOfMeals { get; set; }
        public decimal DailyCalories { get; set; }

        //public int CustomerId { get; set; }
        [Required]
        public List<AddMealDTO> Meals { get; set; }

        [FutureDateOnly(ErrorMessage = "Start date must be update")]
        public DateOnly StartDate { get; set; }

        public int Id { get; set; }
    }
}
