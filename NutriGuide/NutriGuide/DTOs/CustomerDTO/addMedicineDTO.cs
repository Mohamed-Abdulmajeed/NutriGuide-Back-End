using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.CustomerDTO
{
    public class addMedicineDTO
    {
        [Required]
        [MaxLength(20,ErrorMessage ="Length Name must be less than 20")]
        public string? MedicineName { get; set; }

        [RegularExpression("^(After-Food|Before-Food|With-Food)$", ErrorMessage = "Option Must be either 'After-Food','Before-Food','With-Food'")]
        public string? Option { get; set; }

        [Required(ErrorMessage ="Time is required")]
        public List<TimeOnly>? Times { get; set; }
    }
}
