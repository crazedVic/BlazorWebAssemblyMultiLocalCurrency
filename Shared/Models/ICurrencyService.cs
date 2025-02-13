namespace BlazorHelloWorld.Shared.Models;

public interface ICurrencyService
{
    string CurrentCurrency { get; }
    event Action? CurrencyChanged;
    Task<List<CurrencyInfo>> GetAvailableCurrencies();
    Task SetCurrentCurrency(string currency);
    Task<decimal> ConvertPrice(decimal price, string fromCurrency, string toCurrency);
    Task<string> FormatPrice(decimal price, string currency);
} 