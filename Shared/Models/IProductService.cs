namespace BlazorHelloWorld.Shared.Models;

public interface IProductService
{
    Task<ProductTranslations> GetProducts();
} 