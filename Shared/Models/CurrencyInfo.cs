namespace BlazorHelloWorld.Shared.Models;

public class CurrencyInfo
{
    private string _code = string.Empty;
    private string _name = string.Empty;
    private string _flagCode = string.Empty;
    private string _symbol = string.Empty;

    public string Code 
    { 
        get => _code;
        set => _code = value ?? string.Empty;
    }

    public string Name 
    { 
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public string FlagCode 
    { 
        get => _flagCode;
        set => _flagCode = value ?? string.Empty;
    }

    public decimal ExchangeRate { get; set; }

    public string Symbol 
    { 
        get => _symbol;
        set => _symbol = value ?? string.Empty;
    }
} 