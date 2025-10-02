using System.ComponentModel.DataAnnotations;

namespace Task5.Models.User;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(1)] // extract defaults
    public string UserPassword { get; init; }  = string.Empty;
}