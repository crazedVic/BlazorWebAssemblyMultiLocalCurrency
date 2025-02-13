using System;
using System.Threading.Tasks;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Shared.Models;

public class Product
{
    private readonly ICurrencyService _currencyService;
    private readonly ILocalizationService _localizationService;
    private readonly ICategoryService _categoryService;
    
    private string _id = string.Empty;
    private string _name = string.Empty;
    private string _category = string.Empty;
    private string _baseCurrency = "USD";
    private string _unit = string.Empty;

    public string Id 
    { 
        get => _id;
        set => _id = value ?? string.Empty;
    }

    public string Name 
    { 
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public string Category 
    { 
        get => _category;
        set => _category = value ?? string.Empty;
    }

    public decimal Price { get; set; }

    public string BaseCurrency 
    { 
        get => _baseCurrency;
        set => _baseCurrency = string.IsNullOrWhiteSpace(value) ? "USD" : value;
    }

    public int StockQuantity { get; set; }

    public string Unit 
    { 
        get => _unit;
        set => _unit = value ?? string.Empty;
    }

    public Product(ILocalizationService localizationService, ICurrencyService currencyService, ICategoryService categoryService)
    {
        _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    public async Task<decimal> GetPriceInCurrency(string currency)
    {
        if (currency is null)
        {
            throw new ArgumentNullException(nameof(currency));
        }
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty or whitespace", nameof(currency));
        }

        return await _currencyService.ConvertPrice(Price, BaseCurrency, currency);
    }

    public async Task<string> GetFormattedPrice(string currency)
    {
        if (currency is null)
        {
            throw new ArgumentNullException(nameof(currency));
        }
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty or whitespace", nameof(currency));
        }

        var price = await GetPriceInCurrency(currency);
        return await _currencyService.FormatPrice(price, currency);
    }

    public string GetLocalizedName(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return Name;
        }

        var translation = _localizationService.GetTranslation(Id, language);
        if (string.IsNullOrWhiteSpace(translation?.Name))
        {
            return Name;
        }

        return translation.Name;
    }

    public string GetLocalizedUnit(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return Unit;
        }

        var translation = _localizationService.GetTranslation(Id, language);
        if (string.IsNullOrWhiteSpace(translation?.Unit))
        {
            return Unit;
        }

        return translation.Unit;
    }

    public async Task<string> GetLocalizedCategory(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return Category;
        }

        if (string.IsNullOrWhiteSpace(Category))
        {
            return string.Empty;
        }

        return await _categoryService.GetCategoryTranslation(Category, language);
    }
} 