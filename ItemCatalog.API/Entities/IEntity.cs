using System.Reflection.Metadata;
namespace ItemCatalog.API.Entities;

public interface IEntity
{
    public Guid Id { get; init; }
}