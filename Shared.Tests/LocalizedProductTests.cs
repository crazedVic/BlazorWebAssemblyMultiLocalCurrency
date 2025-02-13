using Xunit;
using BlazorHelloWorld.Shared.Models;

namespace Shared.Tests;

public class LocalizedProductTests
{
    [Fact]
    public void LocalizedProduct_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var localizedProduct = new LocalizedProduct();

        // Assert
        Assert.Equal(string.Empty, localizedProduct.Name);
        Assert.Equal(string.Empty, localizedProduct.Unit);
    }

    [Fact]
    public void LocalizedProduct_ShouldSetAndGetProperties()
    {
        // Arrange
        var localizedProduct = new LocalizedProduct
        {
            Name = "Producto de prueba",
            Unit = "piezas"
        };

        // Act & Assert
        Assert.Equal("Producto de prueba", localizedProduct.Name);
        Assert.Equal("piezas", localizedProduct.Unit);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("Test", null)]
    [InlineData(null, "pcs")]
    public void LocalizedProduct_ShouldHandleNullAndEmptyValues(string? name, string? unit)
    {
        // Arrange
        var localizedProduct = new LocalizedProduct();
        if (name != null) localizedProduct.Name = name;
        if (unit != null) localizedProduct.Unit = unit;

        // Assert
        Assert.NotNull(localizedProduct.Name);
        Assert.NotNull(localizedProduct.Unit);
    }

    [Fact]
    public void LocalizedProduct_ShouldCreateCopy()
    {
        // Arrange
        var original = new LocalizedProduct
        {
            Name = "Original",
            Unit = "pcs"
        };

        // Act
        var copy = new LocalizedProduct
        {
            Name = original.Name,
            Unit = original.Unit
        };

        // Assert
        Assert.Equal(original.Name, copy.Name);
        Assert.Equal(original.Unit, copy.Unit);
        Assert.NotSame(original, copy);
    }

    [Fact]
    public void LocalizedProduct_ShouldHandleSpecialCharacters()
    {
        // Arrange & Act
        var localizedProduct = new LocalizedProduct
        {
            Name = "Café ñ 漢字",
            Unit = "μ²"
        };

        // Assert
        Assert.Equal("Café ñ 漢字", localizedProduct.Name);
        Assert.Equal("μ²", localizedProduct.Unit);
    }

    [Fact]
    public void LocalizedProduct_ShouldHandleWhitespaceOnly()
    {
        // Arrange & Act
        var localizedProduct = new LocalizedProduct
        {
            Name = "   ",
            Unit = "\t\n"
        };

        // Assert
        Assert.Equal("   ", localizedProduct.Name);
        Assert.Equal("\t\n", localizedProduct.Unit);
    }

    [Fact]
    public void LocalizedProduct_ShouldHandleLongStrings()
    {
        // Arrange
        var longName = new string('a', 1000);
        var longUnit = new string('b', 1000);

        // Act
        var localizedProduct = new LocalizedProduct
        {
            Name = longName,
            Unit = longUnit
        };

        // Assert
        Assert.Equal(longName, localizedProduct.Name);
        Assert.Equal(longUnit, localizedProduct.Unit);
    }
} 