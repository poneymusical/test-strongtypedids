using API.Products.DTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Products.Endpoints;

[ApiController]
public class GetProducts(AppDbContext dbContext) : ControllerBase
{
    [HttpGet("products")]
    [Tags("Products")]
    public async Task<ActionResult<IEnumerable<GetProductDTO>>> Handle()
    {
        var products = await dbContext.Set<Product>().ProjectToDTO().ToListAsync();
        return Ok(products);
    }
}