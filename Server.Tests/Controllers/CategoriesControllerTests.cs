using BlazorHelloWorld.Server.Controllers;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BlazorHelloWorld.Server.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new CategoriesController(_mockCategoryService.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsOkResultWithCategories()
    {
        // Arrange
        var categoryIds = new List<string> { "electronics", "books" };

        _mockCategoryService.Setup(s => s.GetAllCategoryIds())
            .ReturnsAsync(categoryIds);

        // Setup translations for each language
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("electronics", "en"))
            .ReturnsAsync("Electronics");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("electronics", "es"))
            .ReturnsAsync("Electrónicos");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("electronics", "fr"))
            .ReturnsAsync("Électronique");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("electronics", "de"))
            .ReturnsAsync("Elektronik");

        _mockCategoryService.Setup(s => s.GetCategoryTranslation("books", "en"))
            .ReturnsAsync("Books");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("books", "es"))
            .ReturnsAsync("Libros");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("books", "fr"))
            .ReturnsAsync("Livres");
        _mockCategoryService.Setup(s => s.GetCategoryTranslation("books", "de"))
            .ReturnsAsync("Bücher");

        var expectedCategories = new CategoryTranslations
        {
            Categories = new List<CategoryTranslationItem>
            {
                new()
                {
                    Id = "electronics",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Electronics",
                        ["es"] = "Electrónicos",
                        ["fr"] = "Électronique",
                        ["de"] = "Elektronik"
                    }
                },
                new()
                {
                    Id = "books",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Books",
                        ["es"] = "Libros",
                        ["fr"] = "Livres",
                        ["de"] = "Bücher"
                    }
                }
            }
        };

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsType<CategoryTranslations>(okResult.Value);
        Assert.Equal(expectedCategories.Categories.Count, returnedCategories.Categories.Count);
        Assert.Equal(expectedCategories.Categories[0].Id, returnedCategories.Categories[0].Id);
        Assert.Equal(expectedCategories.Categories[0].Translations["en"], returnedCategories.Categories[0].Translations["en"]);
        Assert.Equal(expectedCategories.Categories[1].Id, returnedCategories.Categories[1].Id);
        Assert.Equal(expectedCategories.Categories[1].Translations["en"], returnedCategories.Categories[1].Translations["en"]);
    }

    [Fact]
    public async Task GetCategories_WhenNoCategories_ReturnsEmptyList()
    {
        // Arrange
        var categoryIds = new List<string>();

        _mockCategoryService.Setup(s => s.GetAllCategoryIds())
            .ReturnsAsync(categoryIds);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsType<CategoryTranslations>(okResult.Value);
        Assert.Empty(returnedCategories.Categories);
    }

    [Fact]
    public async Task GetCategories_WhenServiceThrowsException_Returns500Error()
    {
        // Arrange
        _mockCategoryService.Setup(s => s.GetAllCategoryIds())
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("An error occurred while retrieving categories", statusResult.Value);
    }
} 