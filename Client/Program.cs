using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorHelloWorld;
using BlazorHelloWorld.Client.Data;
using BlazorHelloWorld.Data;
using BlazorHelloWorld.Shared.Models;
using System.Globalization;
using Microsoft.JSInterop;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient 
    { BaseAddress = new Uri("http://localhost:5051") }); // Use your actual server API port

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<BlazorHelloWorld.Shared.Models.ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddSingleton<CounterState>();

// Configure supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("es-ES"),
    new CultureInfo("fr-FR"),
    new CultureInfo("de-DE")
};

CultureInfo.DefaultThreadCurrentCulture = supportedCultures[0];
CultureInfo.DefaultThreadCurrentUICulture = supportedCultures[0];

builder.Services.AddLocalization();
builder.Services.AddMudServices();

var host = builder.Build();

var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");

if (!string.IsNullOrEmpty(result))
{
    var culture = new CultureInfo(result);
    if (supportedCultures.Any(c => c.Name == culture.Name))
    {
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
else
{
    await jsInterop.InvokeVoidAsync("blazorCulture.set", "en-US");
}

await host.RunAsync();
