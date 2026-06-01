using System.ComponentModel.DataAnnotations;

namespace Vcm.Application.DTOs.Categories;

public class CategoryRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
