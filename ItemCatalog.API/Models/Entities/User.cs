using System.ComponentModel.DataAnnotations;
namespace ItemCatalog.API.Models.Entities;

public class User
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid E-mail.")]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}