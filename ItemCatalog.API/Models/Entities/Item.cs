namespace ItemCatalog.API.Models.Entities;
public record Item : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DateTimeOffset CreatedDate { get; set; }
}
