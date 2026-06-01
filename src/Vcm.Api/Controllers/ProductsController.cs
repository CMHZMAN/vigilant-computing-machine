using Microsoft.AspNetCore.Mvc;
using Vcm.Application.DTOs;
using Vcm.Application.DTOs.Products;
using Vcm.Application.Interfaces.Services;

namespace Vcm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedResult<ProductResponseDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest(new { message = "Page and pageSize must be greater than zero." });
        return Ok(await productService.GetPagedAsync(page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(ProductRequestDto request)
    {
        try
        {
            var created = await productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductRequestDto request)
    {
        try
        {
            var updated = await productService.UpdateAsync(id, request);
            return updated ? NoContent() : NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await productService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
