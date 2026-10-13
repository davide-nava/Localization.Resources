# Localization.Resources

A high-performance, enterprise-grade localization and globalization package for **.NET 11**, providing shared, strongly-typed resource files (`.resx`) and localization assets for .NET applications.

---

## Package Overview

**Localization.Resources** centralizes application strings, model validation constraints, and operational error messages across modern .NET architectures. It eliminates magic strings and decentralized localization files by providing:

- **Shared Strongly-Typed Resource Accessors**: Auto-generated, compile-time safe classes (`GenericResource`, `ErrorResource`, `ValidationResource`) generated from `.resx` files.
- **Out-of-the-Box Multi-Lingual Support**: Bundled translations for **English (`en`)**, **German (`de`)**, **French (`fr`)**, and **Italian (`it`)**, with automatic invariant and neutral language fallback.
- **Clean Architecture & CQRS Compatibility**: Seamlessly integrates into domain validation exceptions, MediatR pipeline behaviors, FluentValidation validators, and API error formatting.
- **ASP.NET Core & DI Ready**: Integrates directly with `Microsoft.Extensions.Localization` and `IStringLocalizer<T>`.
- **Zero Third-Party Runtime Overhead**: Built purely on top of the native .NET Base Class Library (`System.Resources`, `System.Globalization`) and `Microsoft.Extensions.Localization.Abstractions`.

---

## Installation

Install via the .NET CLI:

```bash
dotnet add package Localization.Resources
```

Or via the Package Manager Console in Visual Studio:

```powershell
Install-Package Localization.Resources
```

---

## Quick Start & Code Examples

### 1. Accessing Strongly-Typed Resources

Access localized strings directly using compile-time safe properties:

```csharp
using Localization.Resources;

// Generic application labels
string account = GenericResource.Account;
string addEmail = GenericResource.AddEmail;

// Standard operational error messages
string generalError = ErrorResource.Error;
string notFound = ErrorResource.Error404Title;

// Model validation constraints
string employeeRequired = ValidationResource.EmployeeIdRequired;
```

### 2. Multi-Lingual Culture Resolution & Switching

Resources automatically resolve using the ambient thread UI culture (`CultureInfo.CurrentUICulture`), with fallback to neutral languages and invariant defaults. You can also explicitly override the culture on any resource:

```csharp
using System.Globalization;
using Localization.Resources;

// Set ambient UI culture for the current thread
CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
Console.WriteLine(GenericResource.Account); // German localized string

// Or explicitly override culture for a specific resource
GenericResource.Culture = new CultureInfo("it-IT");
Console.WriteLine(GenericResource.Account); // Italian localized string

// Reset back to ambient thread UI culture
GenericResource.Culture = null;
```

### 3. Integrating with CQRS & FluentValidation

```csharp
using FluentValidation;
using Localization.Resources;

public sealed record RegisterEmployeeCommand(string? EmployeeId, string? Reason);

public sealed class RegisterEmployeeCommandValidator : AbstractValidator<RegisterEmployeeCommand>
{
    public RegisterEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage(_ => ValidationResource.EmployeeIdRequired);

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage(_ => ValidationResource.MandatoryReasonRequired);
    }
}
```

### 4. ASP.NET Core & `IStringLocalizer` Integration

```csharp
using Localization.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization();

var app = builder.Build();

app.MapGet("/api/welcome", (IStringLocalizer<GenericResource> localizer) =>
{
    string label = localizer["Account"];
    return Results.Ok(new { Message = label });
});

app.Run();
```

---

## Requirements & Target Frameworks

- **Target Framework**: `.NET 11.0` (`net11.0`)
- **Language Version**: C# 14 / Preview
- **Supported Platforms**: Cross-platform (Windows, Linux, macOS)
- **External Dependencies**: Zero third-party runtime dependencies

---

## Contributing

We welcome contributions! Whether you want to add support for a new culture (e.g. Spanish, Portuguese), add common domain resource keys, or improve documentation, please visit our GitHub repository:

👉 [GitHub Repository & Contributing Guidelines](https://github.com/davide-nava/Localization.Resources)

---

## License

This project is licensed under the [MIT License](LICENSE) - see the `LICENSE` file for details.

## Disclaimer

This software is provided "as is", without warranty of any kind, express or implied. In no event shall the author or copyright holders be liable for any claim, damages, or other liability arising from, out of, or in connection with the software or the use or other dealings in the software.

This project is intended for informational and development purposes. Anyone deploying, hosting, or integrating this software is solely responsible for verifying its suitability, ensuring data security and system integrity, and maintaining compliance with all applicable laws and regulations (including privacy and data protection standards).
