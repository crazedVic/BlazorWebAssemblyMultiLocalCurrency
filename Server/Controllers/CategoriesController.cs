using Microsoft.AspNetCore.Mvc;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;

namespace BlazorHelloWorld.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<CategoryTranslations>> GetCategories()
    {
        try
        {
            var categoryIds = await _categoryService.GetAllCategoryIds();
            var categories = new List<CategoryTranslationItem>();

            foreach (var id in categoryIds)
            {
                var translations = new Dictionary<string, string>();
                foreach (var lang in new[] { "en", "es", "fr", "de" })
                {
                    var translation = await _categoryService.GetCategoryTranslation(id, lang);
                    if (!string.IsNullOrEmpty(translation))
                    {
                        translations[lang] = translation;
                    }
                }

                categories.Add(new CategoryTranslationItem
                {
                    Id = id,
                    Translations = translations
                });
            }

            return Ok(new CategoryTranslations { Categories = categories });
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving categories");
        }
    }
} 