using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;
using Xunit;

namespace Shared.Tests;

public class TestProductService : BaseProductService
{
    private bool _shouldThrow;
    private bool _returnNullProducts;

    public TestProductService(bool shouldThrow = false, bool returnNullProducts = false)
    {
        _shouldThrow = shouldThrow;
        _returnNullProducts = returnNullProducts;
        if (!returnNullProducts)
        {
            ProductTranslations = new ProductTranslations
            {
                Products = new List<ProductTranslationItem>
                {
                    new()
                    {
                        Id = "laptop",
                        Category = "electronics",
                        Price = 999.99m,
                        BaseCurrency = "USD",
                        StockQuantity = 10,
                        Translations = new Dictionary<string, LocalizedProduct>
                        {
                            ["en"] = new LocalizedProduct { Name = "Laptop", Unit = "piece" },
                            ["es"] = new LocalizedProduct { Name = "Portátil", Unit = "pieza" }
                        }
                    },
                    new()
                    {
                        Id = "book",
                        Category = "books",
                        Price = 29.99m,
                        BaseCurrency = "USD",
                        StockQuantity = 50,
                        Translations = new Dictionary<string, LocalizedProduct>
                        {
                            ["en"] = new LocalizedProduct { Name = "Programming Book", Unit = "piece" },
                            ["es"] = new LocalizedProduct { Name = "Libro de Programación", Unit = "pieza" }
                        }
                    }
                }
            };
        }
    }

    protected override Task EnsureProductsLoaded()
    {
        if (_shouldThrow)
            throw new Exception("Test exception");
        if (_returnNullProducts)
            ProductTranslations = null;
        return Task.CompletedTask;
    }
}

public class BaseProductServiceTests
{
    private readonly TestProductService _service;

    public BaseProductServiceTests()
    {
        _service = new TestProductService();
    }

    [Fact]
    public async Task GetProducts_ReturnsAllProducts()
    {
        // Act
        var products = await _service.GetProducts();

        // Assert
        Assert.Equal(2, products.Products.Count);
        
        var laptop = products.Products.First(p => p.Id == "laptop");
        Assert.Equal("electronics", laptop.Category);
        Assert.Equal(999.99m, laptop.Price);
        Assert.Equal("USD", laptop.BaseCurrency);
        Assert.Equal(10, laptop.StockQuantity);
        Assert.Equal("Laptop", laptop.Translations["en"].Name);
        Assert.Equal("piece", laptop.Translations["en"].Unit);
        Assert.Equal("Portátil", laptop.Translations["es"].Name);
        Assert.Equal("pieza", laptop.Translations["es"].Unit);

        var book = products.Products.First(p => p.Id == "book");
        Assert.Equal("books", book.Category);
        Assert.Equal(29.99m, book.Price);
        Assert.Equal("USD", book.BaseCurrency);
        Assert.Equal(50, book.StockQuantity);
        Assert.Equal("Programming Book", book.Translations["en"].Name);
        Assert.Equal("piece", book.Translations["en"].Unit);
        Assert.Equal("Libro de Programación", book.Translations["es"].Name);
        Assert.Equal("pieza", book.Translations["es"].Unit);
    }

    [Fact]
    public async Task GetProducts_WhenLoadFails_ThrowsException()
    {
        // Arrange
        var service = new TestProductService(shouldThrow: true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetProducts());
    }

    [Fact]
    public async Task GetProducts_WhenProductsNotLoaded_ThrowsInvalidOperationException()
    {
        // Arrange
        var service = new TestProductService(returnNullProducts: true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetProducts());
    }
} 