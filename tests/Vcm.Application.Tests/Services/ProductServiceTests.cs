using NSubstitute;
using Vcm.Application.DTOs.Products;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Application.Services;
using Vcm.Domain.Entities;

namespace Vcm.Application.Tests.Services;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    [Fact]
    public async Task GetAllAsync_WhenProductsExist_ReturnsMappedProducts()
    {
        // Arrange
        var service = new ProductService(_productRepository, _categoryRepository);
        _productRepository.GetAllWithCategoryAsync().Returns([
            new Product { Id = 1, Name = "Phone", Price = 999m, CategoryId = 2, Category = new Category { Id = 2, Name = "Tech" } }
        ]);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Tech", result[0].CategoryName);
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryExists_ReturnsCreatedProduct()
    {
        // Arrange
        var service = new ProductService(_productRepository, _categoryRepository);
        var request = new ProductRequestDto { Name = "Phone", Price = 100m, CategoryId = 1 };
        _categoryRepository.ExistsAsync(1).Returns(true);
        _productRepository.AddAsync(Arg.Any<Product>())
            .Returns(callInfo =>
            {
                var product = callInfo.Arg<Product>();
                product.Id = 7;
                return product;
            });
        _productRepository.GetByIdWithCategoryAsync(7)
            .Returns(new Product { Id = 7, Name = "Phone", Price = 100m, CategoryId = 1, Category = new Category { Id = 1, Name = "Tech" } });

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal(7, result.Id);
        Assert.Equal("Tech", result.CategoryName);
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var service = new ProductService(_productRepository, _categoryRepository);
        _categoryRepository.ExistsAsync(42).Returns(false);

        // Act
        var act = () => service.CreateAsync(new ProductRequestDto { Name = "Phone", Price = 100m, CategoryId = 42 });

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var service = new ProductService(_productRepository, _categoryRepository);
        _categoryRepository.ExistsAsync(1).Returns(true);
        _productRepository.GetByIdAsync(88).Returns((Product?)null);

        // Act
        var result = await service.UpdateAsync(88, new ProductRequestDto { Name = "x", Price = 1, CategoryId = 1 });

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_DeletesAndReturnsTrue()
    {
        // Arrange
        var service = new ProductService(_productRepository, _categoryRepository);
        var product = new Product { Id = 5, Name = "Keyboard", Price = 10m, CategoryId = 1 };
        _productRepository.GetByIdAsync(5).Returns(product);

        // Act
        var result = await service.DeleteAsync(5);

        // Assert
        Assert.True(result);
        await _productRepository.Received(1).DeleteAsync(product);
    }
}
