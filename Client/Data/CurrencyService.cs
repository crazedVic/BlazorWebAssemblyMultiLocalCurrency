using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Globalization;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Client.Data;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private string _currentCurrency = "USD";
    private Dictionary<string, CurrencyInfo> _currencies = new();
    
    public event Action? CurrencyChanged;
    
    public string CurrentCurrency => _currentCurrency;

    private class FormatPriceResponse
    {
        public string FormattedPrice { get; set; } = string.Empty;
    }

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CurrencyInfo>> GetAvailableCurrencies()
    {
        var currencies = await _httpClient.GetFromJsonAsync<List<CurrencyInfo>>("api/Currency");
        if (currencies != null)
        {
            _currencies = currencies.ToDictionary(c => c.Code);
        }
        return currencies ?? new List<CurrencyInfo>();
    }

    public async Task SetCurrentCurrency(string currency)
    {
        if (_currentCurrency != currency)
        {
            _currentCurrency = currency;
            CurrencyChanged?.Invoke();
        }
        await Task.CompletedTask;
    }

    public async Task<decimal> ConvertPrice(decimal price, string fromCurrency, string toCurrency)
    {
        if (!_currencies.ContainsKey(fromCurrency) || !_currencies.ContainsKey(toCurrency))
        {
            return price;
        }

        var fromRate = _currencies[fromCurrency].ExchangeRate;
        var toRate = _currencies[toCurrency].ExchangeRate;

        // Convert to USD first (as it's our base currency with rate 1.0), then to target currency
        var usdAmount = price / fromRate;
        var convertedAmount = usdAmount * toRate;

        return Math.Round(convertedAmount, 2);
    }

    public async Task<string> FormatPrice(decimal price, string currency)
    {
        if (!_currencies.TryGetValue(currency, out var currencyInfo))
        {
            return price.ToString("N2", CultureInfo.InvariantCulture);
        }

        // Format with the currency symbol in the appropriate position
        // Most currencies show symbol before the amount (like $10.00)
        // Some currencies might need symbol after, but for simplicity we'll put all before for now
        return $"{currencyInfo.Symbol}{price.ToString("N2", CultureInfo.InvariantCulture)}";
    }
} 