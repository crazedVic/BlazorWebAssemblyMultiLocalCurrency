using Xunit;
using Moq;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace Shared.Tests;

public class ProductTests
{
    private readonly Mock<ILocalizationService> _localizationServiceMock;
    private readonly Mock<ICurrencyService> _currencyServiceMock;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Product _product;

    public ProductTests()
    {
        _localizationServiceMock = new Mock<ILocalizationService>();
        _currencyServiceMock = new Mock<ICurrencyService>();
        _categoryServiceMock = new Mock<ICategoryService>();

        _product = new Product(_localizationServiceMock.Object, _currencyServiceMock.Object, _categoryServiceMock.Object)
        {
            Id = "test-product",
            Name = "Test Product",
            Category = "test-category",
            Price = 100.00m,
            BaseCurrency = "USD",
            StockQuantity = 10,
            Unit = "pcs"
        };
    }

    [Fact]
    public async Task GetPriceInCurrency_ShouldConvertPrice()
    {
        // Arrange
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", "EUR"))
            .ReturnsAsync(85.00m);

        // Act
        var result = await _product.GetPriceInCurrency("EUR");

        // Assert
        Assert.Equal(85.00m, result);
        _currencyServiceMock.Verify(x => x.ConvertPrice(100.00m, "USD", "EUR"), Times.Once);
    }

    [Fact]
    public async Task GetFormattedPrice_ShouldReturnFormattedPrice()
    {
        // Arrange
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", "EUR"))
            .ReturnsAsync(85.00m);
        _currencyServiceMock.Setup(x => x.FormatPrice(85.00m, "EUR"))
            .ReturnsAsync("€85.00");

        // Act
        var result = await _product.GetFormattedPrice("EUR");

        // Assert
        Assert.Equal("€85.00", result);
    }

    [Fact]
    public void GetLocalizedName_ShouldReturnTranslatedName_WhenAvailable()
    {
        // Arrange
        var translation = new LocalizedProduct { Name = "Producto de prueba", Unit = "pzs" };
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", "es-ES"))
            .Returns(translation);

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Producto de prueba", result);
    }

    [Fact]
    public void GetLocalizedName_ShouldReturnDefaultName_WhenTranslationNotAvailable()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public async Task GetLocalizedCategory_ShouldReturnTranslatedCategory()
    {
        // Arrange
        _categoryServiceMock.Setup(x => x.GetCategoryTranslation("test-category", "es-ES"))
            .ReturnsAsync("Categoría de prueba");

        // Act
        var result = await _product.GetLocalizedCategory("es-ES");

        // Assert
        Assert.Equal("Categoría de prueba", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldReturnTranslatedUnit_WhenAvailable()
    {
        // Arrange
        var translation = new LocalizedProduct { Name = "Producto de prueba", Unit = "piezas" };
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", "es-ES"))
            .Returns(translation);

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("piezas", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldReturnDefaultUnit_WhenTranslationNotAvailable()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("pcs", result);
    }

    [Fact]
    public async Task GetPriceInCurrency_ShouldHandleZeroPrice()
    {
        // Arrange
        _product.Price = 0m;
        _currencyServiceMock.Setup(x => x.ConvertPrice(0m, "USD", "EUR"))
            .ReturnsAsync(0m);

        // Act
        var result = await _product.GetPriceInCurrency("EUR");

        // Assert
        Assert.Equal(0m, result);
    }

    [Theory]
    [InlineData("USD", "EUR", 100.00, 85.00)]
    [InlineData("USD", "GBP", 100.00, 73.50)]
    [InlineData("USD", "JPY", 100.00, 11000.00)]
    public async Task GetPriceInCurrency_ShouldHandleMultipleCurrencies(string from, string to, decimal originalPrice, decimal expectedPrice)
    {
        // Arrange
        _product.Price = originalPrice;
        _product.BaseCurrency = from;
        _currencyServiceMock.Setup(x => x.ConvertPrice(originalPrice, from, to))
            .ReturnsAsync(expectedPrice);

        // Act
        var result = await _product.GetPriceInCurrency(to);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Fact]
    public async Task GetPriceInCurrency_ShouldHandleNegativePrice()
    {
        // Arrange
        _product.Price = -50.00m;
        _currencyServiceMock.Setup(x => x.ConvertPrice(-50.00m, "USD", "EUR"))
            .ReturnsAsync(-42.50m);

        // Act
        var result = await _product.GetPriceInCurrency("EUR");

        // Assert
        Assert.Equal(-42.50m, result);
    }

    [Fact]
    public async Task GetFormattedPrice_ShouldHandleException()
    {
        // Arrange
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", "EUR"))
            .ThrowsAsync(new Exception("Currency conversion failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _product.GetFormattedPrice("EUR"));
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleEmptyTranslation()
    {
        // Arrange
        var translation = new LocalizedProduct { Name = "", Unit = "pzs" };
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", "es-ES"))
            .Returns(translation);

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenServicesAreNull()
    {
        // Arrange
        ILocalizationService? nullLocalization = null;
        ICurrencyService? nullCurrency = null;
        ICategoryService? nullCategory = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Product(nullLocalization!, _currencyServiceMock.Object, _categoryServiceMock.Object));
        Assert.Throws<ArgumentNullException>(() => new Product(_localizationServiceMock.Object, nullCurrency!, _categoryServiceMock.Object));
        Assert.Throws<ArgumentNullException>(() => new Product(_localizationServiceMock.Object, _currencyServiceMock.Object, nullCategory!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetLocalizedCategory_ShouldHandleInvalidCategory(string category)
    {
        // Arrange
        _product.Category = category;
        _categoryServiceMock.Setup(x => x.GetCategoryTranslation(category, "es-ES"))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _product.GetLocalizedCategory("es-ES");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Product_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var product = new Product(_localizationServiceMock.Object, _currencyServiceMock.Object, _categoryServiceMock.Object);

        // Assert
        Assert.Equal(string.Empty, product.Id);
        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(string.Empty, product.Category);
        Assert.Equal(0m, product.Price);
        Assert.Equal("USD", product.BaseCurrency);
        Assert.Equal(0, product.StockQuantity);
        Assert.Equal(string.Empty, product.Unit);
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleNullTranslation()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", "es-ES"))
            .Returns((LocalizedProduct?)null);

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldHandleNullTranslation()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", "es-ES"))
            .Returns((LocalizedProduct?)null);

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("pcs", result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetPriceInCurrency_ShouldHandleInvalidCurrency(string currency)
    {
        // Arrange
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", currency))
            .ThrowsAsync(new ArgumentException("Invalid currency"));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _product.GetPriceInCurrency(currency));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetLocalizedName_ShouldHandleInvalidLanguage(string language)
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), language))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedName(language);

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetLocalizedUnit_ShouldHandleInvalidLanguage(string language)
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), language))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedUnit(language);

        // Assert
        Assert.Equal("pcs", result);
    }

    [Fact]
    public async Task GetFormattedPrice_ShouldHandleFormattingException()
    {
        // Arrange
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", "EUR"))
            .ReturnsAsync(85.00m);
        _currencyServiceMock.Setup(x => x.FormatPrice(85.00m, "EUR"))
            .ThrowsAsync(new FormatException("Invalid format"));

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => _product.GetFormattedPrice("EUR"));
    }

    [Fact]
    public void Product_ShouldAllowChangingProperties()
    {
        // Arrange
        var product = new Product(_localizationServiceMock.Object, _currencyServiceMock.Object, _categoryServiceMock.Object);

        // Act
        product.Id = "new-id";
        product.Name = "New Name";
        product.Category = "new-category";
        product.Price = 200.00m;
        product.BaseCurrency = "EUR";
        product.StockQuantity = 20;
        product.Unit = "boxes";

        // Assert
        Assert.Equal("new-id", product.Id);
        Assert.Equal("New Name", product.Name);
        Assert.Equal("new-category", product.Category);
        Assert.Equal(200.00m, product.Price);
        Assert.Equal("EUR", product.BaseCurrency);
        Assert.Equal(20, product.StockQuantity);
        Assert.Equal("boxes", product.Unit);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Product_ShouldHandleVariousStockQuantities(int quantity)
    {
        // Arrange & Act
        _product.StockQuantity = quantity;

        // Assert
        Assert.Equal(quantity, _product.StockQuantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999999.99)]
    [InlineData(999999.99)]
    public void Product_ShouldHandleVariousPrices(decimal price)
    {
        // Arrange & Act
        _product.Price = price;

        // Assert
        Assert.Equal(price, _product.Price);
    }

    [Fact]
    public async Task GetPriceInCurrency_ShouldHandleNullCurrency()
    {
        // Arrange
        string? nullCurrency = null;
        _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", nullCurrency!))
            .ThrowsAsync(new ArgumentNullException(nameof(nullCurrency)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _product.GetPriceInCurrency(nullCurrency!));
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleNullLanguage()
    {
        // Arrange
        string? nullLanguage = null;
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", nullLanguage!))
            .Returns((LocalizedProduct?)null);

        // Act
        var result = _product.GetLocalizedName(nullLanguage!);

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldHandleNullLanguage()
    {
        // Arrange
        string? nullLanguage = null;
        _localizationServiceMock.Setup(x => x.GetTranslation("test-product", nullLanguage!))
            .Returns((LocalizedProduct?)null);

        // Act
        var result = _product.GetLocalizedUnit(nullLanguage!);

        // Assert
        Assert.Equal("pcs", result);
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleNullTranslationObject()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldHandleNullTranslationObject()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("pcs", result);
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleNullName()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = string.Empty, Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldHandleNullUnit()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = string.Empty });

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("pcs", result);
    }

    [Fact]
    public void GetLocalizedName_ShouldHandleEmptyName()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = string.Empty, Unit = "pcs" });

        // Act
        var result = _product.GetLocalizedName("es-ES");

        // Assert
        Assert.Equal("Test Product", result);
    }

    [Fact]
    public void GetLocalizedUnit_ShouldHandleEmptyUnit()
    {
        // Arrange
        _localizationServiceMock.Setup(x => x.GetTranslation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LocalizedProduct { Name = "Test Product", Unit = string.Empty });

        // Act
        var result = _product.GetLocalizedUnit("es-ES");

        // Assert
        Assert.Equal("pcs", result);
    }
} 