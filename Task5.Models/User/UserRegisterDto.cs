using System.ComponentModel.DataAnnotations;

namespace Task5.Models.User;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    public string UserName { get; init; } = string.Empty;

    public string UserPosition { get; init; }  = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    public string UserPassword { get; init; }  = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    [Compare("UserPassword", ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}