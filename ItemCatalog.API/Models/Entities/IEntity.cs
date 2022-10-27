using System.Reflection.Metadata;
namespace ItemCatalog.API.Models.Entities;

public interface IEntity
{
    public Guid Id { get; set; }
}