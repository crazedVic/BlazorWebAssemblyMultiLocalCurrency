using Microsoft.AspNetCore.Mvc;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;

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
        var products = await _productService.GetProducts();
        return Ok(products);
    }
} 