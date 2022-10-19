namespace ItemCatalog.API.Entities;

public record User : IEntity
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public byte[] PasswordHash { get; init; }
    public byte[] PasswordSalt { get; init; }

}