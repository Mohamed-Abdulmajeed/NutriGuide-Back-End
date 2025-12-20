using System.ComponentModel.DataAnnotations;

namespace NutriGuide.DTOs.userDTOs
{
    public class registerDTO
    {
        [Required(ErrorMessage ="Name is required")]
        [MinLength(3,ErrorMessage ="Name Must be at least 3 char")]
        [MaxLength(100,ErrorMessage ="Name Must be less than 100")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6,ErrorMessage ="Password Must be at least 6 char")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Admin|Customer)$",ErrorMessage ="Role Must be either Admin or Customer")]
        public string Role { get; set; } = null!;
        [MaxLength(15, ErrorMessage = "enter phone true")]
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; } = null!;

    }
}
