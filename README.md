# Blazor WebAssembly Curosr AI Composer Demo

![Blazor Hello World Demo Screenshot](docs/screen.png)

A modern e-commerce demonstration application built with Blazor WebAssembly, showcasing various features and best practices in .NET development.

## Project Creation

This entire solution was architected and implemented by Claude 3.5 Sonnet LLM using the Cursor AI Composer, demonstrating the capabilities of AI-assisted software development. Every aspect of the application - from architecture decisions to code implementation - was generated through AI pair programming.

## Features

- **Multi-currency Support**: Dynamic currency conversion and formatting
- **Internationalization (i18n)**: Multi-language support with resource files
- **Product Management**: Product catalog with categories
- **Modern Architecture**: Clean separation of concerns with Client-Server architecture

## Technical Stack

- **.NET 9 Preview**
- **Blazor WebAssembly** for the client-side application
- **ASP.NET Core** for the backend API
- **Entity Framework Core** for data access
- **RESTful API** architecture

## Third-Party Libraries & Frameworks

### UI Components & Styling
- **MudBlazor (v8.2.0)**: Material Design component library for Blazor
- **Bootstrap**: Frontend CSS framework for responsive design
- **Flag Icons (v6.11.0)**: CSS library for country flag icons
- **Google Fonts**: Roboto font family for consistent typography

### Core Dependencies
- **Microsoft.AspNetCore.Components.WebAssembly (v9.0.0-preview.1.24081.5)**: Core Blazor WebAssembly framework
- **Microsoft.Extensions.Localization (v9.0.1)**: Localization support for multi-language features
- **Microsoft.AspNetCore.OpenApi (v9.0.0)**: OpenAPI (Swagger) support for API documentation

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
4. Run the application:
   ```bash
   dotnet run --project Server
   ```

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

## Contributing

Feel free to submit issues and enhancement requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
