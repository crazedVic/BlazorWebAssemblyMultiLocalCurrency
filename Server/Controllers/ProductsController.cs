using Microsoft.AspNetCore.Mvc;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;
using BlazorHelloWorld.Shared.Services;

namespace BlazorHelloWorld.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ProductTranslations>> GetProducts()
    {
        try
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving products");
        }
    }
} 