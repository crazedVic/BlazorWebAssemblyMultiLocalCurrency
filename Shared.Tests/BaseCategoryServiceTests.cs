using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;
using Xunit;

namespace Shared.Tests;

public class TestCategoryService : BaseCategoryService
{
    private bool _shouldThrow;

    public TestCategoryService(bool shouldThrow = false)
    {
        _shouldThrow = shouldThrow;
        CategoryTranslations = new CategoryTranslations
        {
            Categories = new List<CategoryTranslationItem>
            {
                new()
                {
                    Id = "electronics",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Electronics",
                        ["en-US"] = "Electronics (US)",
                        ["es"] = "Electrónicos",
                        ["fr"] = "Électronique"
                    }
                },
                new()
                {
                    Id = "books",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Books",
                        ["es"] = "Libros",
                        ["fr"] = "Livres"
                    }
                }
            }
        };
    }

    protected override Task EnsureCategoriesLoaded()
    {
        if (_shouldThrow)
            throw new Exception("Test exception");
        return Task.CompletedTask;
    }
}

public class BaseCategoryServiceTests
{
    private readonly TestCategoryService _service;

    public BaseCategoryServiceTests()
    {
        _service = new TestCategoryService();
    }

    [Fact]
    public async Task GetAllCategoryIds_ReturnsAllIds()
    {
        // Act
        var ids = await _service.GetAllCategoryIds();

        // Assert
        Assert.Equal(2, ids.Count());
        Assert.Contains("electronics", ids);
        Assert.Contains("books", ids);
    }

    [Theory]
    [InlineData("electronics", "en", "Electronics")]
    [InlineData("electronics", "en-US", "Electronics (US)")]
    [InlineData("electronics", "es", "Electrónicos")]
    [InlineData("electronics", "fr", "Électronique")]
    [InlineData("books", "en", "Books")]
    [InlineData("books", "es", "Libros")]
    [InlineData("books", "fr", "Livres")]
    public async Task GetCategoryTranslation_ReturnsCorrectTranslation(string categoryId, string language, string expected)
    {
        // Act
        var translation = await _service.GetCategoryTranslation(categoryId, language);

        // Assert
        Assert.Equal(expected, translation);
    }

    [Fact]
    public async Task GetCategoryTranslation_WithInvalidCategory_ReturnsCategoryId()
    {
        // Act
        var translation = await _service.GetCategoryTranslation("invalid", "en");

        // Assert
        Assert.Equal("invalid", translation);
    }

    [Theory]
    [InlineData("electronics", "de")]
    [InlineData("books", "it")]
    public async Task GetCategoryTranslation_WithMissingTranslation_ReturnsEnglishFallback(string categoryId, string language)
    {
        // Act
        var translation = await _service.GetCategoryTranslation(categoryId, language);

        // Assert
        var expectedEnglishTranslation = categoryId == "electronics" ? "Electronics" : "Books";
        Assert.Equal(expectedEnglishTranslation, translation);
    }

    [Theory]
    [InlineData("en-US", "en")]
    [InlineData("es-MX", "es")]
    [InlineData("fr-FR", "fr")]
    public async Task GetCategoryTranslation_WithRegionalLanguage_FallsBackToBaseLanguage(string regionalLanguage, string baseLanguage)
    {
        // Arrange
        var categoryId = "books";
        var baseTranslation = await _service.GetCategoryTranslation(categoryId, baseLanguage);

        // Act
        var regionalTranslation = await _service.GetCategoryTranslation(categoryId, regionalLanguage);

        // Assert
        Assert.Equal(baseTranslation, regionalTranslation);
    }

    [Fact]
    public async Task GetAllCategoryTranslations_ReturnsAllTranslationsForLanguage()
    {
        // Act
        var translations = await _service.GetAllCategoryTranslations("es");

        // Assert
        Assert.Equal(2, translations.Count);
        Assert.Equal("Electrónicos", translations["electronics"]);
        Assert.Equal("Libros", translations["books"]);
    }

    [Fact]
    public async Task GetAllCategoryTranslations_WithMissingLanguage_FallsBackToEnglish()
    {
        // Act
        var translations = await _service.GetAllCategoryTranslations("de");

        // Assert
        Assert.Equal(2, translations.Count);
        Assert.Equal("Electronics", translations["electronics"]);
        Assert.Equal("Books", translations["books"]);
    }

    [Fact]
    public async Task GetAllCategoryIds_WhenLoadFails_ThrowsException()
    {
        // Arrange
        var service = new TestCategoryService(shouldThrow: true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetAllCategoryIds());
    }
} 