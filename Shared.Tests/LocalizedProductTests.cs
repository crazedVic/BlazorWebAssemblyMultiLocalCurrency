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
} 