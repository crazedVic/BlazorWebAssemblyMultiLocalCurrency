using System.Net.Http.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Client.Data;

public class CategoryService : BaseCategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected override async Task EnsureCategoriesLoaded()
    {
        if (CategoryTranslations != null) return;

        CategoryTranslations = await _httpClient.GetFromJsonAsync<CategoryTranslations>("/api/categories")
            ?? throw new InvalidOperationException("Failed to load categories.json");
    }
}