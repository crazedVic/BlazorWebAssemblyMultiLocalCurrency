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

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData("", "", "", "")]
    [InlineData(" ", " ", " ", " ")]
    [InlineData("USD", null, "us", "$")]
    public void CurrencyInfo_ShouldHandleNullAndEmptyValues(string? code, string? name, string? flagCode, string? symbol)
    {
        // Arrange
        var currencyInfo = new CurrencyInfo
        {
            Code = code,
            Name = name,
            FlagCode = flagCode,
            Symbol = symbol
        };

        // Assert
        Assert.NotNull(currencyInfo.Code);
        Assert.NotNull(currencyInfo.Name);
        Assert.NotNull(currencyInfo.FlagCode);
        Assert.NotNull(currencyInfo.Symbol);
    }

    [Fact]
    public void CurrencyInfo_ShouldCreateCopy()
    {
        // Arrange
        var original = new CurrencyInfo
        {
            Code = "USD",
            Name = "US Dollar",
            FlagCode = "us",
            ExchangeRate = 1.0m,
            Symbol = "$"
        };

        // Act
        var copy = new CurrencyInfo
        {
            Code = original.Code,
            Name = original.Name,
            FlagCode = original.FlagCode,
            ExchangeRate = original.ExchangeRate,
            Symbol = original.Symbol
        };

        // Assert
        Assert.Equal(original.Code, copy.Code);
        Assert.Equal(original.Name, copy.Name);
        Assert.Equal(original.FlagCode, copy.FlagCode);
        Assert.Equal(original.ExchangeRate, copy.ExchangeRate);
        Assert.Equal(original.Symbol, copy.Symbol);
        Assert.NotSame(original, copy);
    }

    [Theory]
    [InlineData(0.0001)]
    [InlineData(1234567.8901)]
    [InlineData(-0.0001)]
    [InlineData(-1234567.8901)]
    public void CurrencyInfo_ShouldHandleExchangeRatePrecision(double rate)
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { ExchangeRate = (decimal)rate };

        // Assert
        Assert.Equal((decimal)rate, currencyInfo.ExchangeRate);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleMaxValues()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { ExchangeRate = decimal.MaxValue };

        // Assert
        Assert.Equal(decimal.MaxValue, currencyInfo.ExchangeRate);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleMinValues()
    {
        // Arrange & Act
        var currencyInfo = new CurrencyInfo { ExchangeRate = decimal.MinValue };

        // Assert
        Assert.Equal(decimal.MinValue, currencyInfo.ExchangeRate);
    }

    [Fact]
    public void CurrencyInfo_ShouldHandleLongStrings()
    {
        // Arrange
        var longName = new string('a', 1000);

        // Act
        var currencyInfo = new CurrencyInfo { Name = longName };

        // Assert
        Assert.Equal(longName, currencyInfo.Name);
    }
} 