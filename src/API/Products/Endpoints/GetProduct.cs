using System.ComponentModel.DataAnnotations;
using API.Products.DTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Products.Endpoints;

[ApiController]
public class GetProduct(AppDbContext dbContext) : ControllerBase
{
    [HttpGet("product/{id}")]
    [Tags("Products")]
    public async Task<ActionResult<GetProductDTO>> Handle([Required] Guid id)
    {
        var product = await dbContext.Set<Product>().ProjectToDTO().FirstOrDefaultAsync(p => p.Id.Equals(id));
        return product is not null ? Ok(product) : NotFound();
    }
}