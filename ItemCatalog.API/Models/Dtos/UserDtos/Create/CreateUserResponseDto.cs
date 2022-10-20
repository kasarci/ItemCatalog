namespace ItemCatalog.API.Models.Dtos.UserDtos.Create;

public record CreateUserResponseDto
{
    public string Username { get; set; }
    public string Email { get; set; }
}