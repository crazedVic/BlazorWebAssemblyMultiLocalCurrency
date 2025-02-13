using System.Collections.Generic;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Shared.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    List<(string Code, string Name)> AvailableLanguages { get; }
    event Action? LanguageChanged;
    Task SetLanguage(string language);
    void AddTranslation(string key, string language, LocalizedProduct translation);
    LocalizedProduct GetTranslation(string key, string language);
} 