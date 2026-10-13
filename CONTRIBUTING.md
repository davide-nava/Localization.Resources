# Contributing to Localization.Resources

Thank you for your interest in contributing to **Localization.Resources**! We are delighted to welcome open-source contributors from all backgrounds. Whether you are fixing a typo, improving translations, adding support for a new culture, proposing new resource keys, or enhancing documentation and tests, your help makes this library better for the entire .NET community.

---

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [About Localization.Resources](#about-localizationresources)
- [Ways to Contribute](#ways-to-contribute)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Working with Resource Files (.resx)](#working-with-resource-files-resx)
- [Engineering Standards & Quality Gates](#engineering-standards--quality-gates)
- [Submitting a Pull Request](#submitting-a-pull-request)
- [Community & Questions](#community--questions)

---

## Code of Conduct

We are committed to providing a friendly, welcoming, and inclusive environment for everyone. When contributing or participating in discussions:
- Be respectful, empathetic, and constructive.
- Welcome newcomers and encourage diverse perspectives.
- Focus on collaboration and helping others succeed.

---

## About Localization.Resources

**Localization.Resources** provides shared, strongly-typed resource files (`.resx`) and localization assets for .NET applications. It centralizes common UI labels, operational errors, and model validation constraints across modern .NET architectures (such as Clean Architecture, CQRS, and Minimal APIs), eliminating decentralized strings and magic text.

The primary resource dictionaries are:
- `GenericResource`: Common UI labels, standard action names, prompt text, and application concepts.
- `ErrorResource`: Operational errors, HTTP status message descriptors, and system fault messages.
- `ValidationResource`: Model validation constraints, input requirement notifications, and domain rule violations.

Currently supported cultures out-of-the-box:
- Neutral / Invariant / Default: English (`en`)
- German (`de`)
- French (`fr`)
- Italian (`it`)

---

## Ways to Contribute

You can contribute in several impactful ways:

1. **Improve Existing Translations**: Refine translations in English, German, French, or Italian for greater natural phrasing and linguistic accuracy.
2. **Add New Cultures**: Propose and contribute translations for additional languages (such as Spanish `es`, Portuguese `pt`, Japanese `ja`, etc.).
3. **Add New Resource Strings**: Suggest new keys for generic UI labels, validation rules, or operational errors commonly needed across .NET enterprise projects.
4. **Improve Documentation & Examples**: Expand usage scenarios, architectural patterns, integration guides (e.g. FluentValidation, MediatR, ASP.NET Core), and quickstart docs.
5. **Report Bugs & Suggest Enhancements**: Submit detailed bug reports, edge cases, or ideas for improving the library.

---

## Getting Started

### Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download) (or preview runtime supporting .NET 11)
- Git 2.x+
- A modern IDE or editor: Visual Studio 2026 / 2022, JetBrains Rider, or Visual Studio Code with the C# Dev Kit extension.

### Fork & Clone

1. Fork the repository on GitHub: [davide-nava/Localization.Resources](https://github.com/davide-nava/Localization.Resources)
2. Clone your fork locally:
   ```bash
   git clone https://github.com/<your-username>/Localization.Resources.git
   cd Localization.Resources
   ```
3. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```

---

## Development Workflow

### 1. Build the Solution

Restore dependencies and build the entire solution:

```bash
dotnet build Localization.Resources.slnx
```

Our build enables `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`. All builds must finish with **0 Warning(s)** and **0 Error(s)**.

### 2. Run the Unit Tests

Execute the unit test suite and verify code coverage:

```bash
dotnet test Localization.Resources.slnx /p:CollectCoverage=true
```

All tests must pass with a 100% success rate, and code coverage must remain at **100%** on lines, branches, and methods.

---

## Working with Resource Files (.resx)

When adding or modifying localized strings, follow these localization invariants:

1. **Synchronization Across Cultures**:
   - Every string added to the root resource file (e.g. `GenericResource.resx`) must be synchronized across all localized satellite files (`GenericResource.en.resx`, `GenericResource.de.resx`, `GenericResource.fr.resx`, `GenericResource.it.resx`).
   - If a translation for a specific language is pending, supply a sensible fallback or English translation until a native speaker can refine it.

2. **Valid XML Structure**:
   - Ensure `.resx` files maintain valid XML structure, sorted key ordering, and accurate schema definitions.
   - Avoid invalid characters or broken XML tags.

3. **Designer Class Generation**:
   - Strongly-typed accessors in `*.Designer.cs` are generated automatically from `.resx` files.
   - Rebuilding the project regenerates the compile-time safe properties.

4. **Add Unit Tests**:
   - Add corresponding test assertions in the corresponding test suite (`GenericResourceTests.cs`, `ErrorResourceTests.cs`, or `ValidationResourceTests.cs`) verifying that the new resource property returns valid, non-empty text across all supported cultures.

---

## Engineering Standards & Quality Gates

To maintain high architectural standards, this repository enforces strict quality guidelines:

- **Zero-Warning Policy**: All compiler and Roslyn analyzer warnings (`CA*`, `IDE*`, `SA*`, `S*`, `RCS*`, `MA*`, `CS*`) break the build.
- **Language & Documentation**: All code, identifiers, commit messages, and documentation must be in English. Every public or internal member must include comprehensive XML documentation comments.
- **Header Standards**: Maintain the standard file header across all C# source files:
  ```csharp
  // <copyright file="<FileName>.cs" company="Localization.Resources Contributors">
  // Copyright (c) Localization.Resources Contributors. All rights reserved.
  // </copyright>
  ```
- **Modern C# 14 / .NET 11 Idioms**: Use file-scoped namespaces, collection expressions, pattern matching, primary constructors, and non-nullable reference types. Avoid null-suppression operators (`!`) outside of verified test assertions.
- **100% Code Coverage**: Coverlet verifies 100% branch, line, and method coverage on every test run.

---

## Submitting a Pull Request

1. **Keep Changes Focused**: Each pull request should address a single feature, bug fix, or documentation update.
2. **Verify Before Submitting**:
   - Run `dotnet build Localization.Resources.slnx` (0 warnings, 0 errors).
   - Run `dotnet test Localization.Resources.slnx /p:CollectCoverage=true` (100% pass, 100% coverage).
3. **Write Clear Commit Messages**: Use clear, imperative messages (e.g. `Add Spanish translations for ValidationResource`, `Update README examples for CQRS`).
4. **Open the Pull Request**:
   - Push your branch to your GitHub fork: `git push -u origin feature/your-feature-name`.
   - Open a PR against the `main` branch of `davide-nava/Localization.Resources`.
   - Describe what the PR accomplishes, linking any relevant GitHub issues.

---

## Community & Questions

If you have questions, encounter an issue, or want to propose a significant change before writing code:
- Open a GitHub Issue for bug reports or feature requests.
- Start a discussion in the repository's GitHub Discussions tab.

Thank you for helping make **Localization.Resources** great for everyone!
