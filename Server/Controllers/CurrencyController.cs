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
        try
        {
            return Ok(await _currencyService.GetAvailableCurrencies());
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving currencies");
        }
    }

    [HttpGet("convert")]
    public async Task<ActionResult<decimal>> ConvertPrice([FromQuery] string price, [FromQuery] string? fromCurrency, [FromQuery] string? toCurrency)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency))
            {
                return BadRequest("Currency codes cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(price))
            {
                return BadRequest("Currency codes cannot be empty");
            }

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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while converting the price");
        }
    }

    [HttpGet("format")]
    public async Task<ActionResult<string>> FormatPrice([FromQuery] string price, [FromQuery] string? currency)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                return BadRequest("Currency code cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(price))
            {
                return BadRequest("Currency code cannot be empty");
            }

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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while formatting the price");
        }
    }
} 