namespace Vcm.Frontend.Models;

public class CategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
