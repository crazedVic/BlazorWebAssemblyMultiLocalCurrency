namespace BlazorHelloWorld.Shared.Models;

public class CurrencyInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FlagCode { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public string Symbol { get; set; } = string.Empty;
} 