using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.planDTOs
{
    public class UpdateSystemTypePlanDTO
    {
        [Required(ErrorMessage = "Name of SystemType plan is required")]
        [MaxLength(50, ErrorMessage = "Name of SystemType plan can't exceed 50 characters")]
        public string OldName { get; set; }

        [Required(ErrorMessage = "Name of SystemType plan is required")]
        [MaxLength(50, ErrorMessage = "Name of SystemType plan can't exceed 50 characters")]
        public string NewName { get; set; }
    }
}
