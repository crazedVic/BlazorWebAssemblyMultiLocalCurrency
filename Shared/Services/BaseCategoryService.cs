using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Shared.Services;

public abstract class BaseCategoryService : ICategoryService
{
    protected CategoryTranslations? CategoryTranslations { get; set; }

    // This is the abstract method that Client and Server will implement differently
    protected abstract Task EnsureCategoriesLoaded();

    public async Task<string> GetCategoryTranslation(string categoryId, string language)
    {
        await EnsureCategoriesLoaded();
        
        var category = CategoryTranslations!.Categories
            .FirstOrDefault(c => c.Id.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

        if (category == null)
            return categoryId;

        if (category.Translations.TryGetValue(language, out var translation))
            return translation;

        var twoLetterCode = language.Split('-')[0].ToLower();
        if (category.Translations.TryGetValue(twoLetterCode, out translation))
            return translation;

        return category.Translations.GetValueOrDefault("en", categoryId);
    }

    public async Task<IEnumerable<string>> GetAllCategoryIds()
    {
        await EnsureCategoriesLoaded();
        return CategoryTranslations!.Categories.Select(c => c.Id);
    }

    public async Task<Dictionary<string, string>> GetAllCategoryTranslations(string language)
    {
        await EnsureCategoriesLoaded();
        
        var twoLetterCode = language.Split('-')[0].ToLower();
        return CategoryTranslations!.Categories.ToDictionary(
            c => c.Id,
            c => c.Translations.GetValueOrDefault(language, 
                c.Translations.GetValueOrDefault(twoLetterCode,
                    c.Translations.GetValueOrDefault("en", c.Id)))
        );
    }
} 