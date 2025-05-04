using System.ComponentModel.DataAnnotations;

namespace API.Products.DTOs;

public record PostProductDTO
{
   [Required]
    public required string Name { get; init; }
}