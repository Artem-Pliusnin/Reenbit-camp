using System.ComponentModel.DataAnnotations;

namespace Reenbit.Camp.Frontend.Models.Auth;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8), MaxLength(255)]
    public string Password { get; set; }
}