using System;
namespace ItemCatalog.API.Dtos;

public record UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}