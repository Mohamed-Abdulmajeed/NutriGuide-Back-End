using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.CustomerDTO
{
    public class updateDiseaseDTO
    {
        [MaxLength(20, ErrorMessage = "New Disease Name length less than 20")]
        public string NewDisease { get; set; } = null!;
        [MaxLength(20, ErrorMessage = "Old Disease Name length less than 20")]
        public string OldDisease { get; set; } = null!;
    }
}
