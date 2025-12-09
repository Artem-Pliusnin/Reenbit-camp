using System.ComponentModel.DataAnnotations;

namespace Reenbit.Camp.Frontend.Models.Auth;

public class Login
{
    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; }
    
    [Required, MinLength(8), MaxLength(255)]
    public string Password { get; set; }
}