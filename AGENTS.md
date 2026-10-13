# AGENTS.md - Operational Governance & Architecture Manifesto for Autonomous Agents

This document defines the strict operational boundaries, engineering standards, localization invariants, and verification protocols governing any AI coding assistant or autonomous agent working across the **Localization.Resources** repository.

---

## 1. Solution Architecture & Project Anatomy

Localization.Resources is a multi-project .NET 11 solution providing shared, strongly-typed resource files (`.resx`) and localization assets for .NET applications:

```text
Localization.Resources/
├── Localization.Resources.slnx                 # Visual Studio XML Solution orchestrating all projects
├── Directory.Build.props                  # Global MSBuild rules, Roslynator, StyleCop, SonarAnalyzer, zero-warning gate
├── .editorconfig                          # Centralized Roslyn, Meziantou, and code styling conventions
├── .roslynatorconfig                      # Roslynator analyzer configuration
├── .sonarcloud.properties                 # SonarCloud scanner configuration
├── qodana.yaml                            # JetBrains Qodana code quality profile
├── LICENSE                                # MIT License
├── README.md                              # High-level overview, quickstart, and usage documentation
├── CONTRIBUTING.md                        # Open-source contributor guidelines
│
├── .github/
│   └── workflows/
│       ├── publish-nuget.yml              # NuGet package publishing workflow
│       ├── release.yml                    # Release workflow
│       └── sonarcloud.yml                 # SonarCloud static analysis workflow
│
├── assets/
│   ├── stylecop.json                      # StyleCop analyzer parameters and company headers
│   └── img/                               # Application icons and branding assets
│
├── src/
│   └── Localization.Resources/                 # Core .NET 11 localization class library & NuGet package
│       ├── Localization.Resources.csproj
│       ├── ErrorResource.resx / .Designer.cs (en, de, fr, it)
│       ├── GenericResource.resx / .Designer.cs (en, de, fr, it)
│       └── ValidationResource.resx / .Designer.cs (en, de, fr, it)
│
└── tests/
    └── Localization.Resources.UnitTests/        # Comprehensive unit tests (xUnit, Coverlet, Moq)
        ├── Localization.Resources.UnitTests.csproj
        ├── ErrorResourceTests.cs
        ├── GenericResourceTests.cs
        └── ValidationResourceTests.cs
```

---

## 2. Fundamental Architectural & Localization Invariants

### 2.1 Localization Architecture & Invariants

1. **Strongly-Typed Resource Classes**:
   - Primary application strings are organized into three dedicated resource dictionaries:
     - `GenericResource`: Common UI labels, standard action names, prompt text, and application concepts.
     - `ErrorResource`: Operational errors, HTTP status message descriptors, and system fault messages.
     - `ValidationResource`: Model validation constraints, input requirement notifications, and domain rule violations.
   - Resource classes provide static compile-time properties generated from `.resx` files under the `Localization.Resources` root namespace.
   - Any manual or automated change to `.resx` files must preserve valid XML structure and synchronization across all culture variations.

2. **Multi-Lingual Culture Support & Fallback**:
   - The primary supported cultures are:
     - Neutral / Default: English (`en`)
     - German (`de`)
     - French (`fr`)
     - Italian (`it`)
   - Resource lookups strictly rely on standard .NET satellite resource resolution and fallback hierarchy (`specific culture` -> `neutral culture` -> `root default resource`).

3. **Explicit & Ambient Culture Resolution**:
   - Strongly-typed resource classes automatically query strings based on `CultureInfo.CurrentUICulture`.
   - Each resource class provides a static `Culture` property allowing thread-safe explicit culture overrides for that resource without modifying ambient thread state.

4. **Clean Architecture & CQRS Integration**:
   - `Localization.Resources` serves as the foundational localization asset layer across Domain, Application, and Presentation layers.
   - In CQRS patterns, localized resources must be utilized in MediatR pipeline behaviors, FluentValidation validators, domain exceptions, and API response formatting rather than hardcoding magic strings.

---

### 2.2 Engineering & Code Quality Standards

1. **Zero-Warning Policy (`TreatWarningsAsErrors`)**:
   - The repository configures `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` and `<CodeAnalysisTreatWarningsAsErrors>true</CodeAnalysisTreatWarningsAsErrors>`.
   - The build enforces analyzers for `CA*`, `IDE*`, `SA*`, `S*`, `RCS*`, `MA*`, and `CS*`.
   - Under no circumstances may code be committed that introduces compiler warnings or analyzer violations.

2. **Language & Documentation Mandate**:
   - **English Only**: All identifiers, types, methods, comments, XML docs, exceptions, and documentation must be in English.
   - **Comprehensive XML Documentation**: `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is globally enforced. Every public and internal type, method, parameter, return value, and exception MUST contain clear, well-formed XML documentation comments.
   - Maintain the standard file header:
     ```csharp
     // <copyright file="<FileName>.cs" company="Localization.Resources Contributors">
     // Copyright (c) Localization.Resources Contributors. All rights reserved.
     // </copyright>
     ```

3. **Modern C# 14 / .NET 11 Idioms**:
   - Language version is set to `preview` (`<LangVersion>preview</LangVersion>`).
   - Use file-scoped namespaces (`namespace Localization.Resources;`).
   - Use collection expressions (`[..]`), UTF-8 string literals (`"..."u8`), pattern matching, primary constructors, and target-typed `new()`.
   - Nullable reference types are strictly enabled (`<Nullable>enable</Nullable>`). Null-suppression operators (`!`) must be avoided except in verified test assertions.

---

## 3. Project-Specific Stack Guidelines

### 3.1 Core Library (`src/Localization.Resources`)

- **Dependencies**: Keep zero third-party runtime dependencies. Rely entirely on the .NET Base Class Library (`System.Globalization`, `System.Resources`) and `Microsoft.Extensions.Localization.Abstractions`.
- **Design**: Thread-safe, compile-time safe strongly-typed resource accessors.
- **Performance**: Zero-allocation access patterns where feasible; eliminate unnecessary object allocations during culture switches and string lookups.

### 3.2 Unit Tests (`tests/Localization.Resources.UnitTests`)

- **Test Framework**: xUnit with Moq and Coverlet.
- **Sequential Execution**: Tests modifying static culture state (`GenericResource.Culture`, `CultureInfo.CurrentUICulture`, etc.) must belong to the non-parallel `[Collection("CultureTests")]` test collection.
- **Naming Convention**: Test methods must follow `MethodName_Condition_ExpectedBehavior`.
- **Mandatory Test Cases**:
  - Resource accessors returning valid non-empty strings across all supported cultures (`en`, `de`, `fr`, `it`).
  - Dynamic culture override via `Resource.Culture` property and restoration in `Dispose`.
  - Non-null cached singleton instances returned by `ResourceManager` with verified base names (`Localization.Resources.<ResourceName>`).
  - 100% code coverage across all classes, methods, and branches.

---

## 4. Verification & Testing Protocols

Prior to completing any task or proposing changes, autonomous agents must execute the following verification steps:

```bash
# 1. Solution build with zero errors and zero warnings
dotnet build Localization.Resources.slnx

# 2. Run all unit test suites with 100% code coverage collection
dotnet test Localization.Resources.slnx /p:CollectCoverage=true
```

### Verification Checklist:
- [ ] Build completes with **0 Error(s)** and **0 Warning(s)**.
- [ ] All unit tests pass with 100% success rate.
- [ ] Code coverage reaches **100%** on lines, branches, and methods.
- [ ] Any new or modified public/internal member includes full XML documentation.
- [ ] All Markdown documentation files accurately reflect the current codebase.
