using System.ComponentModel.DataAnnotations;

namespace Task5.Models.User;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    [MaxLength(32, ErrorMessage = "Max length for email is 32 symbols")]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    [MaxLength(32, ErrorMessage = "Max length for Name is 32 symbols")]
    public string UserName { get; init; } = string.Empty;

    [MaxLength(64, ErrorMessage = "Max length for Name is 64 symbols")]
    public string UserPosition { get; init; } = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    public string UserPassword { get; init; }  = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    [Compare("UserPassword", ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}