# Localization.Resources

[![Build & Test](https://img.shields.io/badge/build-passing-brightgreen.svg)](#verification-and-testing)
[![.NET 11](https://img.shields.io/badge/.NET-11.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Coverage: 100%](https://img.shields.io/badge/Coverage-100%25-brightgreen.svg)](#verification-and-testing)
[![Code Quality: Zero--Warning](https://img.shields.io/badge/Code%20Quality-Zero--Warning-brightgreen.svg)](#code-quality--static-analysis)

**Localization.Resources** provides shared, strongly-typed resource files (`.resx`) and localization assets for **.NET 11** applications. It centralizes common UI labels, operational error messages, and model validation constraints, offering compile-time safe accessors across multiple languages (**English (`en`)**, **German (`de`)**, **French (`fr`)**, and **Italian (`it`)**) with seamless .NET satellite resource resolution and fallback.

---

## Key Features

- **Shared Strongly-Typed Resource Accessors**: Auto-generated, compile-time safe properties for `GenericResource`, `ErrorResource`, and `ValidationResource`.
- **Comprehensive Multi-Lingual Localization Assets**: Native localized `.resx` dictionaries covering **English (`en`)**, **German (`de`)**, **French (`fr`)**, and **Italian (`it`)**, with automatic invariant and neutral language fallback.
- **Clean Architecture & CQRS Ready**: Provides standardized error messages, validation rules, and generic UI labels for MediatR pipeline behaviors, FluentValidation validators, domain exceptions, and API response formatting.
- **ASP.NET Core & Dependency Injection Compatible**: Seamlessly interoperates with `Microsoft.Extensions.Localization` and `IStringLocalizer<T>`.
- **Zero Third-Party Runtime Overhead**: Built purely on top of the native .NET Base Class Library (`System.Resources`, `System.Globalization`) and `Microsoft.Extensions.Localization.Abstractions`.
- **Zero-Warning Code Quality**: Strictly adheres to Roslyn, StyleCop, SonarAnalyzer, Meziantou, and Roslynator static analysis rules with `TreatWarningsAsErrors` enabled.
- **100% Code Coverage**: Verified with comprehensive xUnit unit tests across all logic branches and resource lookups.

---

## Solution Architecture

The solution (`Localization.Resources.slnx`) follows a clean, modular structure:

```text
Localization.Resources/
├── Localization.Resources.slnx                 # XML Solution definition
├── Directory.Build.props                  # Centralized MSBuild properties, analyzers, and warning gates
├── .editorconfig                          # Roslyn and StyleCop code style standards
├── LICENSE                                # MIT License
├── README.md                              # Repository overview and usage guide
├── CONTRIBUTING.md                        # Open-source contributor guidelines
├── AGENTS.md                              # Architecture manifesto and agent guidelines
│
├── .github/workflows/
│   ├── publish-nuget.yml                  # Automated NuGet publishing pipeline
│   ├── release.yml                        # Release workflow
│   └── sonarcloud.yml                     # SonarCloud code quality scanner
│
├── assets/
│   ├── stylecop.json                      # StyleCop analyzer parameters
│   └── img/                               # Repository icons and assets
│
├── src/
│   └── Localization.Resources/                 # Core localization class library & NuGet package
│       ├── Localization.Resources.csproj
│       ├── ErrorResource.resx / .Designer.cs (en, de, fr, it)
│       ├── GenericResource.resx / .Designer.cs (en, de, fr, it)
│       └── ValidationResource.resx / .Designer.cs (en, de, fr, it)
│
└── tests/
    └── Localization.Resources.UnitTests/        # Test suite (xUnit, Coverlet, Moq)
        ├── Localization.Resources.UnitTests.csproj
        ├── ErrorResourceTests.cs
        ├── GenericResourceTests.cs
        └── ValidationResourceTests.cs
```

---

## Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download) (or preview runtime supporting .NET 11)
- **Supported Operating Systems**: Windows, Linux, macOS (Cross-platform)

---

## Installation & Setup

Install via the .NET CLI:

```bash
dotnet add package Localization.Resources
```

Or via the Package Manager Console in Visual Studio:

```powershell
Install-Package Localization.Resources
```

---

## Usage Examples

### 1. Strongly-Typed Localized Resource Access

Access localized strings directly using compile-time safe properties:

```csharp
using Localization.Resources;

// Access generic application labels
string accountLabel = GenericResource.Account;
string emailPrompt = GenericResource.AddEmail;

// Access standard error messages
string generalError = ErrorResource.Error;
string notFoundTitle = ErrorResource.Error404Title;

// Access validation messages
string employeeRequired = ValidationResource.EmployeeIdRequired;
```

### 2. Multi-Lingual Culture Resolution & Switching

Localized resources automatically respect the current thread's UI culture (`CultureInfo.CurrentUICulture`) with standard .NET satellite resource resolution and fallback. You can also override the culture explicitly on any resource class:

```csharp
using System.Globalization;
using Localization.Resources;

// Set ambient UI culture for the current thread
CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
Console.WriteLine(GenericResource.Account); // German localized string

// Or explicitly override culture for a specific resource without modifying ambient thread state
GenericResource.Culture = new CultureInfo("it-IT");
Console.WriteLine(GenericResource.Account); // Italian localized string

// Reset back to ambient thread UI culture
GenericResource.Culture = null;
```

### 3. Clean Architecture & CQRS Integration

Integrate `Localization.Resources` into CQRS commands, MediatR pipeline behaviors, and FluentValidation validators:

```csharp
using FluentValidation;
using Localization.Resources;
using MediatR;

public sealed record CreateEmployeeCommand(string? EmployeeId, string? Reason) : IRequest<Unit>;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
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

Because `Localization.Resources` is fully compatible with `Microsoft.Extensions.Localization.Abstractions`, you can inject `IStringLocalizer<GenericResource>` or `IStringLocalizer<ValidationResource>` into services and endpoints:

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
    string message = localizer["Account"];
    return Results.Ok(new { Message = message });
});

app.Run();
```

---

## Verification and Testing

The unit test suite validates:
- Multi-lingual resolution and fallback across `en-US`, `de-DE`, `fr-FR`, and `it-IT`.
- Singleton caching and non-null instance guarantees on `ResourceManager`.
- Dynamic culture override via `Resource.Culture` property.
- 100% property coverage across all strongly-typed resource classes.

Run all tests from the repository root:

```bash
# Build the solution with zero warnings
dotnet build Localization.Resources.slnx

# Run all unit tests with 100% code coverage collection
dotnet test Localization.Resources.slnx /p:CollectCoverage=true
```

---

## Code Quality & Static Analysis

This repository enforces strict code quality gates via `Directory.Build.props`:
- **TreatWarningsAsErrors**: All compiler and analyzer warnings break the build.
- **Enforced Analyzers**:
  - `Microsoft.CodeAnalysis.NetAnalyzers`
  - `SonarAnalyzer.CSharp`
  - `StyleCop.Analyzers`
  - `Roslynator.Analyzers` & `Roslynator.CodeFixes`
  - `Meziantou.Analyzer`
  - `SecurityCodeScan.VS2019`
  - `AsyncFixer`
  - `IDisposableAnalyzers`
- **Zero-Warning Gate**: Clean builds on continuous integration pipelines (GitHub Actions, SonarCloud, Qodana).

---

## Contributing

We warmly welcome contributions from the open-source community!

- **New Translations**: Have a language you would like to add (e.g. Spanish, Portuguese, Japanese)? Contributions of satellite `.resx` files are eagerly invited.
- **New Resource Keys**: Need standard operational error codes, generic UI labels, or common validation messages? Propose them via a pull request.
- **Bug Reports & Improvements**: Find a typo or phrasing issue? Please open an issue or pull request.

Please review our [Contributing Guidelines](CONTRIBUTING.md) for details on development workflow, `.resx` synchronization, and quality gates before submitting a pull request.

---

## License

This project is licensed under the [MIT License](LICENSE) - see the `LICENSE` file for details.

## Disclaimer

This software is provided "as is", without warranty of any kind, express or implied. In no event shall the author or copyright holders be liable for any claim, damages, or other liability arising from, out of, or in connection with the software or the use or other dealings in the software.

This project is intended for informational and development purposes. Anyone deploying, hosting, or integrating this software is solely responsible for verifying its suitability, ensuring data security and system integrity, and maintaining compliance with all applicable laws and regulations (including privacy and data protection standards).
