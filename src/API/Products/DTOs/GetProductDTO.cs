using Domain;
using Riok.Mapperly.Abstractions;

namespace API.Products.DTOs;

public record GetProductDTO
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}

[Mapper]
public static partial class GetProductDTOMapper
{
    public static partial GetProductDTO MapToDTO(this Product car);
    public static partial IQueryable<GetProductDTO> ProjectToDTO(this IQueryable<Product> q);
}
