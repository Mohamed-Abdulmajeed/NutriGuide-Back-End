using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.userDTOs
{
    public class loginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password Must be at least 6 char")]
        public string Password { get; set; } = null!;
    }
}
