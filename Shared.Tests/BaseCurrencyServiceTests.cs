using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;
using Xunit;

namespace Shared.Tests;

public class TestCurrencyService : BaseCurrencyService
{
    private bool _shouldThrow;

    public TestCurrencyService(bool shouldThrow = false)
    {
        _shouldThrow = shouldThrow;
        Currencies = new Dictionary<string, CurrencyInfo>
        {
            ["USD"] = new CurrencyInfo { Code = "USD", Symbol = "$", ExchangeRate = 1.0m },
            ["EUR"] = new CurrencyInfo { Code = "EUR", Symbol = "€", ExchangeRate = 0.85m },
            ["GBP"] = new CurrencyInfo { Code = "GBP", Symbol = "£", ExchangeRate = 0.73m }
        };
    }

    protected override Task LoadCurrencies()
    {
        if (_shouldThrow)
            throw new Exception("Test exception");
        return Task.CompletedTask;
    }

    protected override Task SaveCurrentCurrency(string currency)
    {
        if (_shouldThrow)
            throw new Exception("Test exception");
        return Task.CompletedTask;
    }
}

public class BaseCurrencyServiceTests
{
    private readonly TestCurrencyService _service;

    public BaseCurrencyServiceTests()
    {
        _service = new TestCurrencyService();
    }

    [Fact]
    public async Task GetAvailableCurrencies_ReturnsCurrencyList()
    {
        // Act
        var currencies = await _service.GetAvailableCurrencies();

        // Assert
        Assert.Equal(3, currencies.Count);
        Assert.Contains(currencies, c => c.Code == "USD" && c.Symbol == "$" && c.ExchangeRate == 1.0m);
        Assert.Contains(currencies, c => c.Code == "EUR" && c.Symbol == "€" && c.ExchangeRate == 0.85m);
        Assert.Contains(currencies, c => c.Code == "GBP" && c.Symbol == "£" && c.ExchangeRate == 0.73m);
    }

    [Fact]
    public async Task SetCurrentCurrency_UpdatesCurrentCurrency()
    {
        // Arrange
        var currencyChangedCalled = false;
        _service.CurrencyChanged += () => currencyChangedCalled = true;

        // Act
        await _service.SetCurrentCurrency("EUR");

        // Assert
        Assert.Equal("EUR", _service.CurrentCurrency);
        Assert.True(currencyChangedCalled);
    }

    [Fact]
    public async Task SetCurrentCurrency_WhenSameCurrency_DoesNotTriggerEvent()
    {
        // Arrange
        await _service.SetCurrentCurrency("EUR");
        var currencyChangedCalled = false;
        _service.CurrencyChanged += () => currencyChangedCalled = true;

        // Act
        await _service.SetCurrentCurrency("EUR");

        // Assert
        Assert.Equal("EUR", _service.CurrentCurrency);
        Assert.False(currencyChangedCalled);
    }

    [Theory]
    [InlineData(100, "USD", "EUR", 85)] // USD to EUR
    [InlineData(100, "EUR", "USD", 117.65)] // EUR to USD
    [InlineData(100, "GBP", "EUR", 116.44)] // GBP to EUR
    public async Task ConvertPrice_CalculatesCorrectConversion(decimal price, string from, string to, decimal expected)
    {
        // Act
        var result = await _service.ConvertPrice(price, from, to);

        // Assert
        Assert.Equal(expected, result, 2);
    }

    [Fact]
    public async Task ConvertPrice_WithInvalidCurrency_ReturnsSamePrice()
    {
        // Act
        var result = await _service.ConvertPrice(100, "INVALID", "USD");

        // Assert
        Assert.Equal(100, result);
    }

    [Theory]
    [InlineData(100, "USD", "$100.00")]
    [InlineData(100, "EUR", "€100.00")]
    [InlineData(100, "GBP", "£100.00")]
    public async Task FormatPrice_FormatsWithCurrencySymbol(decimal price, string currency, string expected)
    {
        // Act
        var result = await _service.FormatPrice(price, currency);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task FormatPrice_WithInvalidCurrency_ReturnsPlainFormat()
    {
        // Act
        var result = await _service.FormatPrice(100, "INVALID");

        // Assert
        Assert.Equal("100.00", result);
    }

    [Fact]
    public async Task GetAvailableCurrencies_WhenLoadFails_ThrowsException()
    {
        // Arrange
        var service = new TestCurrencyService(shouldThrow: true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetAvailableCurrencies());
    }

    [Fact]
    public async Task SetCurrentCurrency_WhenSaveFails_ThrowsException()
    {
        // Arrange
        var service = new TestCurrencyService(shouldThrow: true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.SetCurrentCurrency("EUR"));
    }
} 