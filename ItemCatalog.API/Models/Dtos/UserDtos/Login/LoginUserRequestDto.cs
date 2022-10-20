using System.ComponentModel.DataAnnotations;

namespace ItemCatalog.API.Models.Dtos.UserDtos.Login;

public class LoginUserRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid e-mail address.")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
