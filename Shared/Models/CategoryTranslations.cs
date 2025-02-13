using System.Collections.Generic;

namespace BlazorHelloWorld.Shared.Models;

public class CategoryTranslations
{
    public List<CategoryTranslationItem> Categories { get; set; } = new();
}

public class CategoryTranslationItem
{
    public string Id { get; set; } = string.Empty;
    public Dictionary<string, string> Translations { get; set; } = new();
} 