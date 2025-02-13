# Blazor WebAssembly Cursor AI Composer Demo

![Blazor Hello World Demo Screenshot](docs/screen.png)

A modern e-commerce demonstration application built with [Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/), showcasing various features and best practices in .NET development.

## Project Creation

This entire solution was architected and implemented by [Claude 3.5 Sonnet](https://www.anthropic.com/claude) LLM using the [Cursor AI](https://cursor.sh/) Composer, demonstrating the capabilities of AI-assisted software development. Every aspect of the application - from architecture decisions to code implementation - was generated through AI pair programming.

## Features

- **Multi-currency Support**: Dynamic currency conversion and formatting
- **Internationalization (i18n)**: Multi-language support with resource files
- **Product Management**: Product catalog with categories
- **Modern Architecture**: Clean separation of concerns with Client-Server architecture

## Technical Stack

- **[.NET 9 Preview](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)**
- **[Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-9.0#blazor-webassembly)** for the client-side application
- **[ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)** for the backend API
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** for data access
- **[RESTful API](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design)** architecture

## Third-Party Libraries & Frameworks

### UI Components & Styling
- **[MudBlazor](https://mudblazor.com/) (v8.2.0)**: Material Design component library for Blazor
- **[Bootstrap](https://getbootstrap.com/)**: Frontend CSS framework for responsive design
- **[Flag Icons](https://github.com/lipis/flag-icons) (v6.11.0)**: CSS library for country flag icons
- **[Google Fonts - Roboto](https://fonts.google.com/specimen/Roboto)**: Roboto font family for consistent typography

### Core Dependencies
- **[Microsoft.AspNetCore.Components.WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-webassembly) (v9.0.0-preview.1.24081.5)**: Core Blazor WebAssembly framework
- **[Microsoft.Extensions.Localization](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization) (v9.0.1)**: Localization support for multi-language features
- **[Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger) (v9.0.0)**: OpenAPI (Swagger) support for API documentation

## Project Structure

- **Client**: Blazor WebAssembly frontend application
- **Server**: ASP.NET Core backend API
- **Shared**: Common models and interfaces shared between Client and Server

## Key Components

- `CurrencyService`: Handles currency conversion and formatting
- `CategoryService`: Manages product categories
- `ProductsController`: API endpoints for product management
- `CultureSelector`: UI component for language selection
- `CurrencySelector`: UI component for currency selection

## Getting Started

1. Ensure you have the .NET 9 Preview SDK installed
2. Clone the repository
3. Navigate to the project directory
4. Run both the Server and Client projects:

   In terminal 1 (Server):
   ```bash
   dotnet run --project Server
   ```

   In terminal 2 (Client):
   ```bash
   dotnet run --project Client
   ```
5. Open your browser and navigate to the Client URL (typically https://localhost:5001)

## Architecture

The application follows a clean architecture pattern with:
- Separation of concerns between Client and Server
- Shared models for consistency
- RESTful API communication
- Dependency injection for services
- Responsive and modern UI

## Features in Detail

### Currency Management
- Real-time currency conversion
- Formatted price display
- Support for multiple currency symbols
- Cached exchange rates

### Internationalization
- Multiple language support
- Culture-aware formatting
- Resource-based translations

## Testing Implementation

### Shared Project Test Coverage

The Shared project implements comprehensive unit tests using xUnit as the testing framework. The tests follow a systematic approach to ensure code quality and reliability.

#### Test Organization

Tests are organized by model classes in separate files:
- `ProductTests.cs`: Tests for the core Product model
- `CurrencyInfoTests.cs`: Tests for currency-related functionality
- `LocalizedProductTests.cs`: Tests for product localization
- `ProductTranslationsTests.cs`: Tests for product translation collections
- `CategoryTranslationsTests.cs`: Tests for category translation management

#### Testing Approach

1. **Model Testing**
   - Default value initialization
   - Property setters and getters
   - Null handling and validation
   - Edge cases and boundary values
   - Copy behavior and object creation

2. **Service Integration**
   - Mock-based testing using Moq
   - Service dependency injection
   - Exception handling
   - Async operations

3. **Localization Testing**
   - Multi-language support
   - Currency formatting
   - Translation management
   - Cultural variants

4. **Data Validation**
   - Null and empty values
   - Special characters
   - Long string handling
   - Numeric precision
   - Currency exchange rates

#### Key Testing Patterns

1. **Arrange-Act-Assert Pattern**
   ```csharp
   // Arrange
   var product = new Product(...);
   
   // Act
   var result = product.GetLocalizedName("en");
   
   // Assert
   Assert.Equal(expectedName, result);
   ```

2. **Theory-based Testing**
   ```csharp
   [Theory]
   [InlineData("USD", "EUR", 100.00, 85.00)]
   [InlineData("USD", "GBP", 100.00, 73.50)]
   public async Task GetPriceInCurrency_ShouldHandleMultipleCurrencies(...)
   ```

3. **Mock-based Testing**
   ```csharp
   _currencyServiceMock.Setup(x => x.ConvertPrice(100.00m, "USD", "EUR"))
       .ReturnsAsync(85.00m);
   ```

#### Test Coverage Areas

1. **Product Class**
   - Price conversion and formatting
   - Localization of product details
   - Category management
   - Stock handling
   - Service integration

2. **CurrencyInfo Class**
   - Exchange rate handling
   - Currency code validation
   - Symbol management
   - Formatting rules

3. **LocalizedProduct Class**
   - Translation management
   - Cultural variants
   - Default fallbacks

4. **Collections**
   - Product translations
   - Category translations
   - List operations
   - Dictionary management

#### Running Tests

Tests can be run using the .NET CLI:
```bash
dotnet test Shared.Tests/Shared.Tests.csproj
```

Or through Visual Studio's Test Explorer.

#### Test Maintenance

- Tests are designed to be maintainable and readable
- Each test has a clear purpose and description
- Mock services are reusable across test cases
- Common setup is handled in constructor or fixture classes

## Contributing

Feel free to submit issues and enhancement requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
