using System.Collections.Generic;

namespace BlazorHelloWorld.Shared.Models;

public class ProductTranslations
{
    public List<ProductTranslationItem> Products { get; set; } = new();
}

public class ProductTranslationItem
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string BaseCurrency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public Dictionary<string, LocalizedProduct> Translations { get; set; } = new();
} 