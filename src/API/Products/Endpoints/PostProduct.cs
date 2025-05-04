using API.Products.DTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Products.Endpoints;

[ApiController]
public class PostProduct(AppDbContext dbContext) : ControllerBase
{
    [HttpPost("products")]
    [Tags("Products")]
    public async Task<IActionResult> Handle([FromBody] PostProductDTO request)
    {
        var product = new Product(request.Name);
        await dbContext.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction("Handle", "GetProduct", new { id = product.Id }, product);
    }
}