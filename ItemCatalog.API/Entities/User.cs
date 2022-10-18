namespace ItemCatalog.API.Entities;

public record User
{
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

}