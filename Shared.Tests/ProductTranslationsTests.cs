using Xunit;
using BlazorHelloWorld.Shared.Models;

namespace Shared.Tests;

public class ProductTranslationsTests
{
    [Fact]
    public void ProductTranslations_ShouldInitializeWithEmptyList()
    {
        // Arrange & Act
        var translations = new ProductTranslations();

        // Assert
        Assert.NotNull(translations.Products);
        Assert.Empty(translations.Products);
    }

    [Fact]
    public void ProductTranslations_ShouldAddAndRetrieveProducts()
    {
        // Arrange
        var translations = new ProductTranslations();
        var product = new ProductTranslationItem
        {
            Id = "test-product",
            Category = "test-category",
            Price = 100.00m,
            BaseCurrency = "USD",
            StockQuantity = 10,
            Translations = new Dictionary<string, LocalizedProduct>
            {
                ["en"] = new LocalizedProduct { Name = "Test Product", Unit = "pcs" },
                ["es-ES"] = new LocalizedProduct { Name = "Producto de prueba", Unit = "piezas" }
            }
        };

        // Act
        translations.Products.Add(product);

        // Assert
        Assert.Single(translations.Products);
        Assert.Equal(product, translations.Products[0]);
        Assert.Equal(2, translations.Products[0].Translations.Count);
    }

    [Fact]
    public void ProductTranslations_ShouldHandleMultipleProducts()
    {
        // Arrange
        var translations = new ProductTranslations();
        var products = new[]
        {
            new ProductTranslationItem { Id = "product-1" },
            new ProductTranslationItem { Id = "product-2" },
            new ProductTranslationItem { Id = "product-3" }
        };

        // Act
        translations.Products.AddRange(products);

        // Assert
        Assert.Equal(3, translations.Products.Count);
        Assert.Equal("product-1", translations.Products[0].Id);
        Assert.Equal("product-2", translations.Products[1].Id);
        Assert.Equal("product-3", translations.Products[2].Id);
    }

    [Fact]
    public void ProductTranslations_ShouldAllowRemovingProducts()
    {
        // Arrange
        var translations = new ProductTranslations();
        var product = new ProductTranslationItem { Id = "test-product" };
        translations.Products.Add(product);

        // Act
        translations.Products.Remove(product);

        // Assert
        Assert.Empty(translations.Products);
    }
}

public class ProductTranslationItemTests
{
    [Fact]
    public void ProductTranslationItem_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var item = new ProductTranslationItem();

        // Assert
        Assert.Equal(string.Empty, item.Id);
        Assert.Equal(string.Empty, item.Category);
        Assert.Equal(0m, item.Price);
        Assert.Equal("USD", item.BaseCurrency);
        Assert.Equal(0, item.StockQuantity);
        Assert.NotNull(item.Translations);
        Assert.Empty(item.Translations);
    }

    [Fact]
    public void ProductTranslationItem_ShouldManageTranslations()
    {
        // Arrange
        var item = new ProductTranslationItem();
        var enTranslation = new LocalizedProduct { Name = "Test Product", Unit = "pcs" };
        var esTranslation = new LocalizedProduct { Name = "Producto de prueba", Unit = "piezas" };

        // Act
        item.Translations["en"] = enTranslation;
        item.Translations["es-ES"] = esTranslation;

        // Assert
        Assert.Equal(2, item.Translations.Count);
        Assert.Equal(enTranslation, item.Translations["en"]);
        Assert.Equal(esTranslation, item.Translations["es-ES"]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("USD")]
    [InlineData("EUR")]
    public void ProductTranslationItem_ShouldSetBaseCurrency(string currency)
    {
        // Arrange & Act
        var item = new ProductTranslationItem { BaseCurrency = currency };

        // Assert
        Assert.Equal(currency, item.BaseCurrency);
    }

    [Fact]
    public void ProductTranslationItem_ShouldOverwriteExistingTranslation()
    {
        // Arrange
        var item = new ProductTranslationItem();
        var originalTranslation = new LocalizedProduct { Name = "Original", Unit = "pcs" };
        var updatedTranslation = new LocalizedProduct { Name = "Updated", Unit = "pieces" };

        // Act
        item.Translations["en"] = originalTranslation;
        item.Translations["en"] = updatedTranslation;

        // Assert
        Assert.Single(item.Translations);
        Assert.Equal(updatedTranslation, item.Translations["en"]);
    }

    [Fact]
    public void ProductTranslationItem_ShouldHandleNegativeStockQuantity()
    {
        // Arrange & Act
        var item = new ProductTranslationItem { StockQuantity = -10 };

        // Assert
        Assert.Equal(-10, item.StockQuantity);
    }

    [Fact]
    public void ProductTranslationItem_ShouldHandleNegativePrice()
    {
        // Arrange & Act
        var item = new ProductTranslationItem { Price = -100.00m };

        // Assert
        Assert.Equal(-100.00m, item.Price);
    }

    [Fact]
    public void ProductTranslationItem_ShouldRemoveTranslation()
    {
        // Arrange
        var item = new ProductTranslationItem();
        item.Translations["en"] = new LocalizedProduct { Name = "Test", Unit = "pcs" };

        // Act
        var removed = item.Translations.Remove("en");

        // Assert
        Assert.True(removed);
        Assert.Empty(item.Translations);
    }
} 