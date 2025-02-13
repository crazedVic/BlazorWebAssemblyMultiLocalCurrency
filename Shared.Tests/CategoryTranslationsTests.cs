using Xunit;
using BlazorHelloWorld.Shared.Models;

namespace Shared.Tests;

public class CategoryTranslationsTests
{
    [Fact]
    public void CategoryTranslations_ShouldInitializeWithEmptyList()
    {
        // Arrange & Act
        var translations = new CategoryTranslations();

        // Assert
        Assert.NotNull(translations.Categories);
        Assert.Empty(translations.Categories);
    }

    [Fact]
    public void CategoryTranslations_ShouldAddAndRetrieveCategories()
    {
        // Arrange
        var translations = new CategoryTranslations();
        var category = new CategoryTranslationItem
        {
            Id = "test-category",
            Translations = new Dictionary<string, string>
            {
                ["en"] = "Test Category",
                ["es-ES"] = "Categoría de prueba"
            }
        };

        // Act
        translations.Categories.Add(category);

        // Assert
        Assert.Single(translations.Categories);
        Assert.Equal(category, translations.Categories[0]);
        Assert.Equal(2, translations.Categories[0].Translations.Count);
    }

    [Fact]
    public void CategoryTranslations_ShouldHandleMultipleCategories()
    {
        // Arrange
        var translations = new CategoryTranslations();
        var categories = new[]
        {
            new CategoryTranslationItem { Id = "category-1" },
            new CategoryTranslationItem { Id = "category-2" },
            new CategoryTranslationItem { Id = "category-3" }
        };

        // Act
        translations.Categories.AddRange(categories);

        // Assert
        Assert.Equal(3, translations.Categories.Count);
        Assert.Equal("category-1", translations.Categories[0].Id);
        Assert.Equal("category-2", translations.Categories[1].Id);
        Assert.Equal("category-3", translations.Categories[2].Id);
    }

    [Fact]
    public void CategoryTranslations_ShouldAllowRemovingCategories()
    {
        // Arrange
        var translations = new CategoryTranslations();
        var category = new CategoryTranslationItem { Id = "test-category" };
        translations.Categories.Add(category);

        // Act
        translations.Categories.Remove(category);

        // Assert
        Assert.Empty(translations.Categories);
    }

    [Fact]
    public void CategoryTranslations_ShouldClearAllCategories()
    {
        // Arrange
        var translations = new CategoryTranslations();
        translations.Categories.AddRange(new[]
        {
            new CategoryTranslationItem { Id = "category-1" },
            new CategoryTranslationItem { Id = "category-2" }
        });

        // Act
        translations.Categories.Clear();

        // Assert
        Assert.Empty(translations.Categories);
    }
}

public class CategoryTranslationItemTests
{
    [Fact]
    public void CategoryTranslationItem_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var item = new CategoryTranslationItem();

        // Assert
        Assert.Equal(string.Empty, item.Id);
        Assert.NotNull(item.Translations);
        Assert.Empty(item.Translations);
    }

    [Fact]
    public void CategoryTranslationItem_ShouldManageTranslations()
    {
        // Arrange
        var item = new CategoryTranslationItem();

        // Act
        item.Translations["en"] = "Test Category";
        item.Translations["es-ES"] = "Categoría de prueba";

        // Assert
        Assert.Equal(2, item.Translations.Count);
        Assert.Equal("Test Category", item.Translations["en"]);
        Assert.Equal("Categoría de prueba", item.Translations["es-ES"]);
    }

    [Fact]
    public void CategoryTranslationItem_ShouldOverwriteExistingTranslation()
    {
        // Arrange
        var item = new CategoryTranslationItem();
        item.Translations["en"] = "Test Category";

        // Act
        item.Translations["en"] = "Updated Test Category";

        // Assert
        Assert.Single(item.Translations);
        Assert.Equal("Updated Test Category", item.Translations["en"]);
    }

    [Fact]
    public void CategoryTranslationItem_ShouldHandleEmptyTranslations()
    {
        // Arrange
        var item = new CategoryTranslationItem();

        // Act
        item.Translations["en"] = "";
        item.Translations["es-ES"] = string.Empty;

        // Assert
        Assert.Equal(2, item.Translations.Count);
        Assert.Equal("", item.Translations["en"]);
        Assert.Equal(string.Empty, item.Translations["es-ES"]);
    }

    [Fact]
    public void CategoryTranslationItem_ShouldRemoveTranslation()
    {
        // Arrange
        var item = new CategoryTranslationItem();
        item.Translations["en"] = "Test Category";

        // Act
        var removed = item.Translations.Remove("en");

        // Assert
        Assert.True(removed);
        Assert.Empty(item.Translations);
    }

    [Fact]
    public void CategoryTranslationItem_ShouldHandleNonExistentTranslation()
    {
        // Arrange
        var item = new CategoryTranslationItem();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _ = item.Translations["nonexistent"]);
    }
} 