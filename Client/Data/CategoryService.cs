using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Data;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private CategoryTranslations? _categoryTranslations;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task EnsureCategoriesLoaded()
    {
        if (_categoryTranslations != null) return;

        _categoryTranslations = await _httpClient.GetFromJsonAsync<CategoryTranslations>("/api/categories")
            ?? throw new InvalidOperationException("Failed to load categories.json");

        //_categoryTranslations = await _httpClient.GetFromJsonAsync<CategoryTranslations>("Data/localization/categories.json")
        //    ?? throw new InvalidOperationException("Failed to load categories.json");
    }

    public async Task<string> GetCategoryTranslation(string categoryId, string language)
    {
        await EnsureCategoriesLoaded();
        
        var category = _categoryTranslations!.Categories
            .FirstOrDefault(c => c.Id.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

        if (category == null)
            return categoryId; // Fallback to ID if category not found

        // Try full culture code first (e.g., "de-DE")
        if (category.Translations.TryGetValue(language, out var translation))
            return translation;

        // Try two-letter language code (e.g., "de")
        var twoLetterCode = language.Split('-')[0].ToLower();
        if (category.Translations.TryGetValue(twoLetterCode, out translation))
            return translation;

        // Fallback to English or ID
        return category.Translations.GetValueOrDefault("en", categoryId);
    }

    public async Task<IEnumerable<string>> GetAllCategoryIds()
    {
        await EnsureCategoriesLoaded();
        return _categoryTranslations!.Categories.Select(c => c.Id);
    }

    public async Task<Dictionary<string, string>> GetAllCategoryTranslations(string language)
    {
        await EnsureCategoriesLoaded();
        
        var twoLetterCode = language.Split('-')[0].ToLower();
        return _categoryTranslations!.Categories.ToDictionary(
            c => c.Id,
            c => c.Translations.GetValueOrDefault(language, 
                c.Translations.GetValueOrDefault(twoLetterCode,
                    c.Translations.GetValueOrDefault("en", c.Id)))
        );
    }
} 