namespace ItemCatalog.API.Dtos.UserDtos.Create;

public record CreateUserResponseDto
{
    public string Username { get; set; }
    public string Email { get; set; }
}