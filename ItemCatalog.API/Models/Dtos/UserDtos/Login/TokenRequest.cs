using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ItemCatalog.API.Models.Dtos.UserDtos.Login;

public class TokenRequest
{
    [Required]
    public string Token { get; set; }
 
    [Required]
    public string RefreshToken { get; set; }
}