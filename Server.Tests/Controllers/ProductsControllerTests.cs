using BlazorHelloWorld.Server.Controllers;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BlazorHelloWorld.Server.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResultWithProducts()
    {
        // Arrange
        var expectedProducts = new ProductTranslations
        {
            Products = new List<ProductTranslationItem>
            {
                new()
                {
                    Id = "1",
                    Category = "Electronics",
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
                    Id = "2",
                    Category = "Books",
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

        _mockProductService.Setup(s => s.GetProducts())
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<ProductTranslations>(okResult.Value);
        Assert.Equal(expectedProducts.Products.Count, returnedProducts.Products.Count);
        Assert.Equal(expectedProducts.Products[0].Id, returnedProducts.Products[0].Id);
        Assert.Equal(expectedProducts.Products[0].Price, returnedProducts.Products[0].Price);
        Assert.Equal(expectedProducts.Products[0].Category, returnedProducts.Products[0].Category);
        Assert.Equal(expectedProducts.Products[0].Translations["en"].Name, returnedProducts.Products[0].Translations["en"].Name);
        Assert.Equal(expectedProducts.Products[0].Translations["es"].Name, returnedProducts.Products[0].Translations["es"].Name);
    }

    [Fact]
    public async Task GetProducts_WhenNoProducts_ReturnsEmptyList()
    {
        // Arrange
        var emptyProducts = new ProductTranslations
        {
            Products = new List<ProductTranslationItem>()
        };

        _mockProductService.Setup(s => s.GetProducts())
            .ReturnsAsync(emptyProducts);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<ProductTranslations>(okResult.Value);
        Assert.Empty(returnedProducts.Products);
    }

    [Fact]
    public async Task GetProducts_WhenServiceThrowsException_Returns500Error()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProducts())
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("An error occurred while retrieving products", statusResult.Value);
    }
} 