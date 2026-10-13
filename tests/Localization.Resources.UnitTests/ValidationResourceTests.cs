// <copyright file="ValidationResourceTests.cs" company="Localization.Resources Contributors">
// Copyright (c) Localization.Resources Contributors. All rights reserved.
// </copyright>

using System.Globalization;
using System.Reflection;

namespace Localization.Resources.UnitTests;

/// <summary>
/// Unit test suite for <see cref="ValidationResource"/>.
/// </summary>
[Collection("CultureTests")]
public sealed class ValidationResourceTests : IDisposable
{
    private readonly CultureInfo? _originalCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResourceTests"/> class,
    /// preserving the initial resource culture.
    /// </summary>
    public ValidationResourceTests()
    {
        _originalCulture = ValidationResource.Culture;
    }

    /// <summary>
    /// Restores the original resource culture after test execution.
    /// </summary>
    public void Dispose() => ValidationResource.Culture = _originalCulture;

    /// <summary>
    /// Verifies that the internal constructor initializes a non-null instance.
    /// </summary>
    [Fact]
    public void Constructor_InitializesInstanceSuccessfully()
    {
        // Act
        var instance = new ValidationResource();

        // Assert
        Assert.NotNull(instance);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResource.ResourceManager"/> returns a non-null,
    /// correctly named <see cref="System.Resources.ResourceManager"/> singleton instance.
    /// </summary>
    [Fact]
    public void ResourceManager_ReturnsNonNullAndCachedInstance()
    {
        // Act
        var manager1 = ValidationResource.ResourceManager;
        var manager2 = ValidationResource.ResourceManager;

        // Assert
        Assert.NotNull(manager1);
        Assert.Same(manager1, manager2);
        Assert.Equal("Localization.Resources.ValidationResource", manager1.BaseName);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResource.Culture"/> property can be set and retrieved.
    /// </summary>
    [Fact]
    public void Culture_SetAndGet_UpdatesCorrectly()
    {
        // Arrange
        var culture = new CultureInfo("fr-FR");

        // Act
        ValidationResource.Culture = culture;

        // Assert
        Assert.Equal(culture, ValidationResource.Culture);

        // Reset
        ValidationResource.Culture = null;
        Assert.Null(ValidationResource.Culture);
    }

    /// <summary>
    /// Verifies that known localized validation strings return non-empty text.
    /// </summary>
    [Fact]
    public void KnownProperties_ReturnExpectedValues()
    {
        // Act & Assert
        Assert.False(string.IsNullOrWhiteSpace(ValidationResource.EmployeeIdRequired));
        Assert.False(string.IsNullOrWhiteSpace(ValidationResource.MandatoryReasonRequired));
    }

    /// <summary>
    /// Verifies that all public static string properties of <see cref="ValidationResource"/>
    /// can be accessed without exception and return valid non-null strings.
    /// </summary>
    [Fact]
    public void AllProperties_ExecuteSuccessfullyAndReturnNonNullValues()
    {
        // Arrange
        PropertyInfo[] properties =
        [
            .. typeof(ValidationResource)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(string)),
        ];

        // Act & Assert
        Assert.NotEmpty(properties);
        foreach (var property in properties)
        {
            var value = (string?)property.GetValue(null);
            Assert.NotNull(value);
            Assert.NotEmpty(value);
        }
    }
}
