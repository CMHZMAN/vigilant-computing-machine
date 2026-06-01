using Microsoft.AspNetCore.Mvc;
using Vcm.Application.DTOs;
using Vcm.Application.DTOs.Categories;
using Vcm.Application.Interfaces.Services;

namespace Vcm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
    {
        var categories = await categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedResult<CategoryResponseDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest(new { message = "Page and pageSize must be greater than zero." });
        return Ok(await categoryService.GetPagedAsync(page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return category is null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(CategoryRequestDto request)
    {
        try
        {
            var created = await categoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequestDto request)
    {
        try
        {
            var updated = await categoryService.UpdateAsync(id, request);
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
        try
        {
            var deleted = await categoryService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}
