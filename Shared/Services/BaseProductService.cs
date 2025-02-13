using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Shared.Services;

public interface IProductService
{
    Task<ProductTranslations> GetProducts();
}

public abstract class BaseProductService : IProductService
{
    protected ProductTranslations? ProductTranslations { get; set; }

    protected abstract Task EnsureProductsLoaded();

    public async Task<ProductTranslations> GetProducts()
    {
        await EnsureProductsLoaded();
        return ProductTranslations ?? throw new InvalidOperationException("Products not loaded.");
    }
} 