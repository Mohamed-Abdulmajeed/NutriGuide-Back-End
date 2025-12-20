using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.CustomerDTO
{
    public class updateFoodDTO
    {
        [MaxLength(20,ErrorMessage ="New Food length less than 20")]
        public string NewFood { get; set; } = null!;
        [MaxLength(20, ErrorMessage = "Old Food length less than 20")]
        public string OldFood { get; set; } = null!;
    }
}
