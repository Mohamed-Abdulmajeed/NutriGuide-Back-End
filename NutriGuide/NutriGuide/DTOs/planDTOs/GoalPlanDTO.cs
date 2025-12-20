using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.planDTOs
{
    public class GoalPlanDTO
    {
        [Required(ErrorMessage = "Name of goal plan is required")]
        [MaxLength(50, ErrorMessage = "Name of goal plan can't exceed 50 characters")]
        public string Name { get; set; }
    }
}
