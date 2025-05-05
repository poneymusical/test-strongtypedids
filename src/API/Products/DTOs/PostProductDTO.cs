using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.Products.DTOs;

public record PostProductDTO
{
    [Required] public required string Name { get; init; }
}