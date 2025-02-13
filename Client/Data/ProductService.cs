using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Client.Data;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrencyService _currencyService;
    private readonly ICategoryService _categoryService;
    private ProductTranslations? _productTranslations;
    
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

    private async Task EnsureProductsLoaded()
    {
        if (_productTranslations is not null) return;

        _productTranslations = await _httpClient.GetFromJsonAsync<ProductTranslations>("/api/products")
            ?? throw new InvalidOperationException("Failed to load products.json");

        // Add translations to the localization service
        foreach (var p in _productTranslations.Products)
        {
            foreach (var kvp in p.Translations)
            {
                _localizationService.AddTranslation(p.Id, kvp.Key, kvp.Value);
            }
        }
    }

    public async Task<ProductTranslations> GetProducts()
    {
        await EnsureProductsLoaded();
        return _productTranslations!;
    }
} 