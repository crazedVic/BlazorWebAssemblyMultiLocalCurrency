using Microsoft.AspNetCore.Mvc;
using BlazorHelloWorld.Server.Services;
using BlazorHelloWorld.Shared.Models;
using System.Globalization;

namespace BlazorHelloWorld.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CurrencyInfo>>> GetAvailableCurrencies()
    {
        return Ok(await _currencyService.GetAvailableCurrencies());
    }

    [HttpGet("convert")]
    public async Task<ActionResult<decimal>> ConvertPrice([FromQuery] string price, [FromQuery] string fromCurrency, [FromQuery] string toCurrency)
    {
        try
        {
            // Parse the price using invariant culture to ensure consistent decimal handling
            if (!decimal.TryParse(price, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsedPrice))
            {
                return BadRequest("Invalid price format");
            }

            var result = await _currencyService.ConvertPrice(parsedPrice, fromCurrency, toCurrency);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("format")]
    public async Task<ActionResult<string>> FormatPrice([FromQuery] string price, [FromQuery] string currency)
    {
        try
        {
            // Parse the price using invariant culture to ensure consistent decimal handling
            if (!decimal.TryParse(price, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsedPrice))
            {
                return BadRequest("Invalid price format");
            }

            var result = await _currencyService.FormatPrice(parsedPrice, currency);
            return Ok(new { formattedPrice = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 