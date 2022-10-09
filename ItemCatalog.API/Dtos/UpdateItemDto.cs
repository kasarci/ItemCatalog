using System.ComponentModel.DataAnnotations;

namespace ItemCatalog.API.Dtos;

public record UpdateItemDto
{
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    [Range(1,9999)]
    public decimal Price { get; init; }
}