using System.Net.Http.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;
using Microsoft.JSInterop;

namespace BlazorHelloWorld.Client.Services;

public class CurrencyService : BaseCurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public CurrencyService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        InitializeCurrency();
    }

    private async void InitializeCurrency()
    {
        var savedCurrency = await _jsRuntime.InvokeAsync<string>("blazorCurrency.get");
        if (!string.IsNullOrEmpty(savedCurrency))
        {
            CurrentCurrencyValue = savedCurrency;
        }
    }

    protected override async Task LoadCurrencies()
    {
        if (Currencies.Count > 0) return;
        
        var currencies = await _httpClient.GetFromJsonAsync<List<CurrencyInfo>>("api/Currency");
        if (currencies != null)
        {
            Currencies = currencies.ToDictionary(c => c.Code);
        }
    }

    protected override async Task SaveCurrentCurrency(string currency)
    {
        await _jsRuntime.InvokeVoidAsync("blazorCurrency.set", currency);
    }
} 