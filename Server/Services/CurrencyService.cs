using System.Text.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Server.Services;

public class CurrencyService : BaseCurrencyService
{
    private readonly string _dataPath;

    public CurrencyService(IWebHostEnvironment webHostEnvironment)
    {
        _dataPath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "currencies.json");
    }

    protected override async Task LoadCurrencies()
    {
        if (Currencies.Count > 0) return;

        if (!File.Exists(_dataPath))
        {
            Currencies = new Dictionary<string, CurrencyInfo>();
            return;
        }

        var jsonString = await File.ReadAllTextAsync(_dataPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var currencies = JsonSerializer.Deserialize<List<CurrencyInfo>>(jsonString, options)
            ?? throw new InvalidOperationException("Failed to deserialize currencies.json");
        
        Currencies = currencies.ToDictionary(c => c.Code);
    }

    protected override Task SaveCurrentCurrency(string currency)
    {
        // Server doesn't need to save the current currency as it's client-specific
        return Task.CompletedTask;
    }
} 