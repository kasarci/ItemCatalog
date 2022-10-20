namespace ItemCatalog.API.Models.Dtos.UserDtos.Remove;

public record RemoveUserRequestDto
{
    public string Username { get; set; }
    public string Email { get; set; }
}