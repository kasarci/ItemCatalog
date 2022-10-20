using System.ComponentModel.DataAnnotations;

namespace ItemCatalog.API.Dtos.RoleDtos;

public record RoleDto
{
    [Required]
    public string Name { get; set; }
}