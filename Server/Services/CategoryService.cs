using System.Text.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Server.Services;

public class CategoryService : BaseCategoryService
{
    private readonly string _dataPath;

    public CategoryService(IWebHostEnvironment webHostEnvironment)
    {
        _dataPath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "categories.json");
    }

    protected override async Task EnsureCategoriesLoaded()
    {
        if (CategoryTranslations != null) return;

        if (!File.Exists(_dataPath))
        {
            CategoryTranslations = new CategoryTranslations { Categories = new List<CategoryTranslationItem>() };
            return;
        }

        var jsonString = await File.ReadAllTextAsync(_dataPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        CategoryTranslations = JsonSerializer.Deserialize<CategoryTranslations>(jsonString, options)
            ?? throw new InvalidOperationException("Failed to deserialize categories.json");
    }
}