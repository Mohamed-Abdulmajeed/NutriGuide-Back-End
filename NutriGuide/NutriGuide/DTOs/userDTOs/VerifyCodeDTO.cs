using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.userDTOs
{
    public class VerifyCodeDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Code is required")]
        [MinLength(6, ErrorMessage = "Code Must be at least 6 char")]
        public string Code { get; set; } = null!;
    }
}
