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
        var translations = new Dictionary<string, string>
        {
            ["electronics"] = "Electronics",
            ["books"] = "Books"
        };

        _mockCategoryService.Setup(s => s.GetAllCategoryIds())
            .ReturnsAsync(categoryIds);
        _mockCategoryService.Setup(s => s.GetAllCategoryTranslations("en"))
            .ReturnsAsync(translations);

        var expectedCategories = new CategoryTranslations
        {
            Categories = new List<CategoryTranslationItem>
            {
                new()
                {
                    Id = "electronics",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Electronics"
                    }
                },
                new()
                {
                    Id = "books",
                    Translations = new Dictionary<string, string>
                    {
                        ["en"] = "Books"
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
        var translations = new Dictionary<string, string>();

        _mockCategoryService.Setup(s => s.GetAllCategoryIds())
            .ReturnsAsync(categoryIds);
        _mockCategoryService.Setup(s => s.GetAllCategoryTranslations("en"))
            .ReturnsAsync(translations);

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