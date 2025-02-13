using Xunit;
using BlazorHelloWorld.Shared.Models;

namespace Shared.Tests;

public class CurrencyInfoTests
{
    [Fact]
    public void CurrencyInfo_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo();

        // Assert
        Assert.Equal(string.Empty, currencyInfo.Code);
        Assert.Equal(string.Empty, currencyInfo.Name);
        Assert.Equal(string.Empty, currencyInfo.FlagCode);
        Assert.Equal(0m, currencyInfo.ExchangeRate);
        Assert.Equal(string.Empty, currencyInfo.Symbol);
    }

    [Fact]
    public void CurrencyInfo_ShouldSetAndGetProperties()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo
        {
            Code = "USD",
            Name = "US Dollar",
            FlagCode = "us",
            ExchangeRate = 1.0m,
            Symbol = "$"
        };

        // Assert
        Assert.Equal("USD", currencyInfo.Code);
        Assert.Equal("US Dollar", currencyInfo.Name);
        Assert.Equal("us", currencyInfo.FlagCode);
        Assert.Equal(1.0m, currencyInfo.ExchangeRate);
        Assert.Equal("$", currencyInfo.Symbol);
    }

    [Theory]
    [InlineData("", "", "", 0.0, "")]
    [InlineData("EUR", "Euro", "eu", 0.85, "€")]
    [InlineData("GBP", "British Pound", "gb", 0.73, "£")]
    public void CurrencyInfo_ShouldHandleVariousValues(string code, string name, string flagCode, double rate, string symbol)
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo
        {
            Code = code,
            Name = name,
            FlagCode = flagCode,
            ExchangeRate = (decimal)rate,
            Symbol = symbol
        };

        // Assert
        Assert.Equal(code, currencyInfo.Code);
        Assert.Equal(name, currencyInfo.Name);
        Assert.Equal(flagCode, currencyInfo.FlagCode);
        Assert.Equal((decimal)rate, currencyInfo.ExchangeRate);
        Assert.Equal(symbol, currencyInfo.Symbol);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleNegativeExchangeRate()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { ExchangeRate = -1.5m };

        // Assert
        Assert.Equal(-1.5m, currencyInfo.ExchangeRate);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleZeroExchangeRate()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { ExchangeRate = 0m };

        // Assert
        Assert.Equal(0m, currencyInfo.ExchangeRate);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleSpecialCharactersInSymbol()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { Symbol = "¥" };

        // Assert
        Assert.Equal("¥", currencyInfo.Symbol);
    }
} 