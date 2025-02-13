namespace BlazorHelloWorld.Shared.Models;

public class Product
{
    private readonly ICurrencyService _currencyService;
    private readonly ILocalizationService _localizationService;
    private readonly ICategoryService _categoryService;
    
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string BaseCurrency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;

    public Product(ILocalizationService localizationService, ICurrencyService currencyService, ICategoryService categoryService)
    {
        _localizationService = localizationService;
        _currencyService = currencyService;
        _categoryService = categoryService;
    }

    public async Task<decimal> GetPriceInCurrency(string currency)
    {
        return await _currencyService.ConvertPrice(Price, BaseCurrency, currency);
    }

    public async Task<string> GetFormattedPrice(string currency)
    {
        var price = await GetPriceInCurrency(currency);
        return await _currencyService.FormatPrice(price, currency);
    }

    public string GetLocalizedName(string language)
    {
        var translation = _localizationService.GetTranslation(Id, language);
        return translation?.Name ?? Name;
    }

    public string GetLocalizedUnit(string language)
    {
        var translation = _localizationService.GetTranslation(Id, language);
        return translation?.Unit ?? Unit;
    }

    public Task<string> GetLocalizedCategory(string language)
    {
        return _categoryService.GetCategoryTranslation(Category, language);
    }
} 