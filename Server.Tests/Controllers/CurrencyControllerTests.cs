using System.Globalization;
using System.Text.Json;
using BlazorHelloWorld.Server.Controllers;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BlazorHelloWorld.Server.Tests.Controllers;

public class CurrencyControllerTests
{
    private readonly Mock<ICurrencyService> _mockCurrencyService;
    private readonly CurrencyController _controller;

    private class FormattedPriceResponse
    {
        public string formattedPrice { get; set; } = string.Empty;
    }

    public CurrencyControllerTests()
    {
        _mockCurrencyService = new Mock<ICurrencyService>();
        _controller = new CurrencyController(_mockCurrencyService.Object);
    }

    [Fact]
    public async Task GetAvailableCurrencies_ReturnsOkResultWithCurrencies()
    {
        // Arrange
        var expectedCurrencies = new List<CurrencyInfo>
        {
            new() { Code = "USD", Symbol = "$" },
            new() { Code = "EUR", Symbol = "€" }
        };
        _mockCurrencyService.Setup(s => s.GetAvailableCurrencies())
            .ReturnsAsync(expectedCurrencies);

        // Act
        var result = await _controller.GetAvailableCurrencies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCurrencies = Assert.IsType<List<CurrencyInfo>>(okResult.Value);
        Assert.Equal(expectedCurrencies, returnedCurrencies);
    }

    [Theory]
    [InlineData("10.50", "USD", "EUR", 8.75)]
    [InlineData("100", "EUR", "USD", 120)]
    public async Task ConvertPrice_WithValidInput_ReturnsOkResultWithConvertedPrice(
        string price, string fromCurrency, string toCurrency, decimal expectedResult)
    {
        // Arrange
        decimal parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
        _mockCurrencyService.Setup(s => s.ConvertPrice(parsedPrice, fromCurrency, toCurrency))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.ConvertPrice(price, fromCurrency, toCurrency);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var convertedPrice = Assert.IsType<decimal>(okResult.Value);
        Assert.Equal(expectedResult, convertedPrice);
    }

    [Theory]
    [InlineData("invalid", "USD", "EUR")]
    [InlineData("abc", "EUR", "USD")]
    public async Task ConvertPrice_WithInvalidPrice_ReturnsBadRequest(
        string price, string fromCurrency, string toCurrency)
    {
        // Act
        var result = await _controller.ConvertPrice(price, fromCurrency, toCurrency);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid price format", badRequestResult.Value);
    }

    [Theory]
    [InlineData("10.50", "USD", "EUR", "Invalid currency")]
    public async Task ConvertPrice_WhenServiceThrowsArgumentException_ReturnsBadRequest(
        string price, string fromCurrency, string toCurrency, string errorMessage)
    {
        // Arrange
        decimal parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
        _mockCurrencyService.Setup(s => s.ConvertPrice(parsedPrice, fromCurrency, toCurrency))
            .ThrowsAsync(new ArgumentException(errorMessage));

        // Act
        var result = await _controller.ConvertPrice(price, fromCurrency, toCurrency);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(errorMessage, badRequestResult.Value);
    }

    [Theory]
    [InlineData("10.50", "USD", "$10.50")]
    [InlineData("100", "EUR", "€100.00")]
    public async Task FormatPrice_WithValidInput_ReturnsOkResultWithFormattedPrice(
        string price, string currency, string expectedResult)
    {
        // Arrange
        decimal parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
        _mockCurrencyService.Setup(s => s.FormatPrice(parsedPrice, currency))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.FormatPrice(price, currency);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var json = JsonSerializer.Serialize(okResult.Value);
        var formattedResult = JsonSerializer.Deserialize<FormattedPriceResponse>(json);
        Assert.NotNull(formattedResult);
        Assert.Equal(expectedResult, formattedResult.formattedPrice);
    }

    [Theory]
    [InlineData("invalid", "USD")]
    [InlineData("abc", "EUR")]
    public async Task FormatPrice_WithInvalidPrice_ReturnsBadRequest(string price, string currency)
    {
        // Act
        var result = await _controller.FormatPrice(price, currency);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid price format", badRequestResult.Value);
    }

    [Theory]
    [InlineData("10.50", "INVALID", "Invalid currency")]
    public async Task FormatPrice_WhenServiceThrowsArgumentException_ReturnsBadRequest(
        string price, string currency, string errorMessage)
    {
        // Arrange
        decimal parsedPrice = decimal.Parse(price, CultureInfo.InvariantCulture);
        _mockCurrencyService.Setup(s => s.FormatPrice(parsedPrice, currency))
            .ThrowsAsync(new ArgumentException(errorMessage));

        // Act
        var result = await _controller.FormatPrice(price, currency);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(errorMessage, badRequestResult.Value);
    }
} 