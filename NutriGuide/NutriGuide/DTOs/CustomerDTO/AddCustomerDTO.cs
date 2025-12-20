using NutriGuide.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.CustomerDTO
{

    public class AddCustomerDTO
    {
        [Required(ErrorMessage = "Birth date must date only")]
        public DateOnly BirthDate { get; set; }
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender Must be either'Male','Female'")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Weight is required")]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Height is required")]
        public double Height { get; set; }
        [Range(1.1,1.9,ErrorMessage ="level must between range 1.1 and 1.9")]
        public decimal ActivityLevel { get; set; }


    }
}
