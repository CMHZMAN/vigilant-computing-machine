using System.Net.Http.Json;
using Vcm.Frontend.Models;

namespace Vcm.Frontend.Services;

public class ApiClientService(HttpClient httpClient) : IApiClientService
{
    public async Task<List<CategoryResponseDto>> GetCategoriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CategoryResponseDto>>("api/categories") ?? [];
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto request)
    {
        var response = await httpClient.PostAsJsonAsync("api/categories", request);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<CategoryResponseDto>() ?? new CategoryResponseDto();
    }

    public async Task UpdateCategoryAsync(int id, CategoryRequestDto request)
    {
        var response = await httpClient.PutAsJsonAsync($"api/categories/{id}", request);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/categories/{id}");
        await EnsureSuccessAsync(response);
    }

    public async Task<List<ProductResponseDto>> GetProductsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ProductResponseDto>>("api/products") ?? [];
    }

    public async Task<ProductResponseDto> CreateProductAsync(ProductRequestDto request)
    {
        var response = await httpClient.PostAsJsonAsync("api/products", request);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<ProductResponseDto>() ?? new ProductResponseDto();
    }

    public async Task UpdateProductAsync(int id, ProductRequestDto request)
    {
        var response = await httpClient.PutAsJsonAsync($"api/products/{id}", request);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/products/{id}");
        await EnsureSuccessAsync(response);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase : body,
            inner: null,
            statusCode: response.StatusCode);
    }
}
