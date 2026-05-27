using NSubstitute;
using Vcm.Application.DTOs.Categories;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Application.Services;
using Vcm.Domain.Entities;

namespace Vcm.Application.Tests.Services;

public class CategoryServiceTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_ReturnsCreatedCategory()
    {
        // Arrange
        var service = new CategoryService(_categoryRepository);
        var request = new CategoryRequestDto { Name = "Electronics" };
        _categoryRepository.AddAsync(Arg.Any<Category>())
            .Returns(callInfo =>
            {
                var category = callInfo.Arg<Category>();
                category.Id = 1;
                return category;
            });

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Electronics", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var service = new CategoryService(_categoryRepository);
        _categoryRepository.GetByIdAsync(99).Returns((Category?)null);

        // Act
        var result = await service.UpdateAsync(99, new CategoryRequestDto { Name = "Updated" });

        // Assert
        Assert.False(result);
        await _categoryRepository.DidNotReceive().UpdateAsync(Arg.Any<Category>());
    }

    [Fact]
    public async Task CreateAsync_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var service = new CategoryService(_categoryRepository);

        // Act
        var act = () => service.CreateAsync(new CategoryRequestDto { Name = "   " });

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }
}
