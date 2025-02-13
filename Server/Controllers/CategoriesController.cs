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
        var categoryIds = await _categoryService.GetAllCategoryIds();
        var translations = await _categoryService.GetAllCategoryTranslations("en");
        
        var categories = categoryIds.Select(id => new CategoryTranslationItem
        {
            Id = id,
            Translations = new Dictionary<string, string>
            {
                ["en"] = translations[id]
            }
        }).ToList();

        return Ok(new CategoryTranslations { Categories = categories });
    }
} 