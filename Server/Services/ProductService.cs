using System.Text.Json;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Server.Services;

public class ProductService : BaseProductService
{
    private readonly string _dataPath;

    public ProductService(IWebHostEnvironment webHostEnvironment)
    {
        _dataPath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "products.json");
    }

    protected override Task EnsureProductsLoaded()
    {
        if (ProductTranslations is not null) return Task.CompletedTask;

        if (!File.Exists(_dataPath))
        {
            ProductTranslations = new ProductTranslations { Products = new List<ProductTranslationItem>() };
            return Task.CompletedTask;
        }

        try
        {
            var jsonString = File.ReadAllText(_dataPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // First deserialize to a dynamic structure to handle numeric IDs
            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;
            var products = new List<ProductTranslationItem>();

            if (root.TryGetProperty("products", out var productsArray))
            {
                foreach (var item in productsArray.EnumerateArray())
                {
                    var product = new ProductTranslationItem
                    {
                        Id = item.GetProperty("id").ToString(), // Convert numeric ID to string
                        Category = item.GetProperty("category").GetString() ?? string.Empty,
                        Price = item.GetProperty("price").GetDecimal(),
                        BaseCurrency = item.GetProperty("baseCurrency").GetString() ?? "USD",
                        StockQuantity = item.GetProperty("stockQuantity").GetInt32(),
                        Translations = new Dictionary<string, LocalizedProduct>()
                    };

                    if (item.TryGetProperty("translations", out var translations))
                    {
                        foreach (var lang in translations.EnumerateObject())
                        {
                            var langCode = lang.Name;
                            var translation = new LocalizedProduct
                            {
                                Name = lang.Value.GetProperty("name").GetString() ?? string.Empty,
                                Unit = lang.Value.GetProperty("unit").GetString() ?? string.Empty
                            };
                            product.Translations[langCode] = translation;
                        }
                    }

                    products.Add(product);
                }
            }

            ProductTranslations = new ProductTranslations { Products = products };
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex}");
            ProductTranslations = new ProductTranslations { Products = new List<ProductTranslationItem>() };
            return Task.CompletedTask;
        }
    }
} 