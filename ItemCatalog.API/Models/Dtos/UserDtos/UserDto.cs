using System.ComponentModel.DataAnnotations;

namespace ItemCatalog.API.Models.Dtos;

public record UserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid e-mail address.")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}