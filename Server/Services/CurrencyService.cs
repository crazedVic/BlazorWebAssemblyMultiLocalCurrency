using System.Collections.Concurrent;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Server.Services;

public class CurrencyService : ICurrencyService
{
    private readonly Dictionary<string, CurrencyInfo> _currencies = new()
    {
        {
            "USD", new CurrencyInfo
            {
                Code = "USD",
                Name = "US Dollar",
                Symbol = "$",
                FlagCode = "us",
                ExchangeRate = 1.0m
            }
        },
        {
            "EUR", new CurrencyInfo
            {
                Code = "EUR",
                Name = "Euro",
                Symbol = "€",
                FlagCode = "eu",
                ExchangeRate = 0.92m
            }
        },
        {
            "AUD", new CurrencyInfo
            {
                Code = "AUD",
                Name = "Australian Dollar",
                Symbol = "A$",
                FlagCode = "au",
                ExchangeRate = 1.53m
            }
        },
        {
            "GBP", new CurrencyInfo
            {
                Code = "GBP",
                Name = "British Pound",
                Symbol = "£",
                FlagCode = "gb",
                ExchangeRate = 0.79m
            }
        },
        {
            "INR", new CurrencyInfo
            {
                Code = "INR",
                Name = "Indian Rupee",
                Symbol = "₹",
                FlagCode = "in",
                ExchangeRate = 83.12m
            }
        },
        {
            "CAD", new CurrencyInfo
            {
                Code = "CAD",
                Name = "Canadian Dollar",
                Symbol = "C$",
                FlagCode = "ca",
                ExchangeRate = 1.35m
            }
        }
    };

    private string _currentCurrency = "USD";
    public event Action? CurrencyChanged;
    public string CurrentCurrency => _currentCurrency;

    public async Task<List<CurrencyInfo>> GetAvailableCurrencies()
    {
        return await Task.FromResult(_currencies.Values.ToList());
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
            return await Task.FromResult(price);
        }

        var fromRate = _currencies[fromCurrency].ExchangeRate;
        var toRate = _currencies[toCurrency].ExchangeRate;

        return await Task.FromResult(price * (toRate / fromRate));
    }

    public async Task<string> FormatPrice(decimal price, string currency)
    {
        if (!_currencies.ContainsKey(currency))
        {
            return await Task.FromResult($"{price:N2}");
        }

        var info = _currencies[currency];
        return await Task.FromResult($"{info.Symbol}{price:N2}");
    }
} 