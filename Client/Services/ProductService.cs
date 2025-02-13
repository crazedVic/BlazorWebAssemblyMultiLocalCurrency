using System.Net.Http.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Client.Services;

public class ProductService : BaseProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrencyService _currencyService;
    private readonly ICategoryService _categoryService;
    
    public ProductService(
        HttpClient httpClient, 
        ILocalizationService localizationService, 
        ICurrencyService currencyService, 
        ICategoryService categoryService)
    {
        _httpClient = httpClient;
        _localizationService = localizationService;
        _currencyService = currencyService;
        _categoryService = categoryService;
    }

    protected override async Task EnsureProductsLoaded()
    {
        if (ProductTranslations is not null) return;

        ProductTranslations = await _httpClient.GetFromJsonAsync<ProductTranslations>("/api/products")
            ?? throw new InvalidOperationException("Failed to load products.json");

        // Add translations to the localization service
        foreach (var p in ProductTranslations.Products)
        {
            foreach (var kvp in p.Translations)
            {
                _localizationService.AddTranslation(p.Id, kvp.Key, kvp.Value);
            }
        }
    }
} 