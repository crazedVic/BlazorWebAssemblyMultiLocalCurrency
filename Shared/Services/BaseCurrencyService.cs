using System;
using System.Collections.Generic;
using System.Globalization;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Shared.Services;

public abstract class BaseCurrencyService : ICurrencyService
{
    protected Dictionary<string, CurrencyInfo> Currencies { get; set; } = new();
    protected string CurrentCurrencyValue = "USD";
    
    public event Action? CurrencyChanged;
    
    public string CurrentCurrency => CurrentCurrencyValue;

    protected abstract Task LoadCurrencies();
    protected abstract Task SaveCurrentCurrency(string currency);

    public async Task<List<CurrencyInfo>> GetAvailableCurrencies()
    {
        await LoadCurrencies();
        return Currencies.Values.ToList();
    }

    public async Task SetCurrentCurrency(string currency)
    {
        if (CurrentCurrencyValue != currency)
        {
            CurrentCurrencyValue = currency;
            await SaveCurrentCurrency(currency);
            CurrencyChanged?.Invoke();
        }
    }

    public Task<decimal> ConvertPrice(decimal price, string fromCurrency, string toCurrency)
    {
        if (!Currencies.ContainsKey(fromCurrency) || !Currencies.ContainsKey(toCurrency))
        {
            return Task.FromResult(price);
        }

        var fromRate = Currencies[fromCurrency].ExchangeRate;
        var toRate = Currencies[toCurrency].ExchangeRate;

        // Convert to USD first (as it's our base currency with rate 1.0), then to target currency
        var usdAmount = price / fromRate;
        var convertedAmount = usdAmount * toRate;

        return Task.FromResult(Math.Round(convertedAmount, 2));
    }

    public Task<string> FormatPrice(decimal price, string currency)
    {
        if (!Currencies.TryGetValue(currency, out var currencyInfo))
        {
            return Task.FromResult(price.ToString("N2", CultureInfo.InvariantCulture));
        }

        return Task.FromResult($"{currencyInfo.Symbol}{price.ToString("N2", CultureInfo.InvariantCulture)}");
    }
} 