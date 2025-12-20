using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.planDTOs
{
    public class SystemTypeDTO
    {
        [Required(ErrorMessage = "Name of system type plan is required")]
        [MaxLength(50, ErrorMessage = "Name of  system type plan can't exceed 50 characters")]
        public string Name { get; set; }
    }
}
