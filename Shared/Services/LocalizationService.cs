using System.Collections.Generic;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Shared.Services;

public class LocalizationService : ILocalizationService
{
    private readonly Dictionary<string, Dictionary<string, LocalizedProduct>> _translations = new()
    {
        ["en"] = new Dictionary<string, LocalizedProduct>(),
        ["fr"] = new Dictionary<string, LocalizedProduct>(),
        ["es"] = new Dictionary<string, LocalizedProduct>(),
        ["de"] = new Dictionary<string, LocalizedProduct>()
    };

    public event Action? LanguageChanged;
    
    public string CurrentLanguage { get; private set; } = "en";
    
    public List<(string Code, string Name)> AvailableLanguages { get; } = new()
    {
        ("en", "English"),
        ("fr", "Français"),
        ("es", "Español"),
        ("de", "Deutsch")
    };

    public Task SetLanguage(string languageCode)
    {
        if (CurrentLanguage != languageCode && _translations.ContainsKey(languageCode))
        {
            CurrentLanguage = languageCode;
            LanguageChanged?.Invoke();
        }
        return Task.CompletedTask;
    }

    public void AddTranslation(string key, string language, LocalizedProduct translation)
    {
        if (_translations.ContainsKey(language))
        {
            _translations[language][key] = translation;
        }
    }

    public LocalizedProduct GetTranslation(string key, string language)
    {
        // Try to get translation in requested language
        if (_translations.ContainsKey(language) && _translations[language].ContainsKey(key))
        {
            return _translations[language][key];
        }
        
        // Fall back to English if translation not found and requested language is not English
        if (language != "en" && _translations["en"].ContainsKey(key))
        {
            return _translations["en"][key];
        }
        
        return new LocalizedProduct(); // Return empty translation if not found
    }
} 