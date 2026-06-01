using System.ComponentModel.DataAnnotations;

namespace Vcm.Application.DTOs.Products;

public class ProductRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}
