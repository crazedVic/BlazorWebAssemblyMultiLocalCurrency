namespace BlazorHelloWorld.Shared.Models;

public interface ICategoryService
{
    Task<string> GetCategoryTranslation(string categoryId, string language);
    Task<IEnumerable<string>> GetAllCategoryIds();
    Task<Dictionary<string, string>> GetAllCategoryTranslations(string language);
} 