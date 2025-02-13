namespace BlazorHelloWorld.Shared.Models;

public class LocalizedProduct
{
    private string _name = string.Empty;
    private string _unit = string.Empty;

    public string Name 
    { 
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public string Unit 
    { 
        get => _unit;
        set => _unit = value ?? string.Empty;
    }
} 