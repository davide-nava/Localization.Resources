// <copyright file="ErrorResourceTests.cs" company="Localization.Resources Contributors">
// Copyright (c) Localization.Resources Contributors. All rights reserved.
// </copyright>

using System.Globalization;
using System.Reflection;

namespace Localization.Resources.UnitTests;

/// <summary>
/// Unit test suite for <see cref="ErrorResource"/>.
/// </summary>
[Collection("CultureTests")]
public sealed class ErrorResourceTests : IDisposable
{
    private readonly CultureInfo? _originalCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResourceTests"/> class,
    /// preserving the initial resource culture.
    /// </summary>
    public ErrorResourceTests()
    {
        _originalCulture = ErrorResource.Culture;
    }

    /// <summary>
    /// Restores the original resource culture after test execution.
    /// </summary>
    public void Dispose() => ErrorResource.Culture = _originalCulture;

    /// <summary>
    /// Verifies that the internal constructor initializes a non-null instance.
    /// </summary>
    [Fact]
    public void Constructor_InitializesInstanceSuccessfully()
    {
        // Act
        var instance = new ErrorResource();

        // Assert
        Assert.NotNull(instance);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorResource.ResourceManager"/> returns a non-null,
    /// correctly named <see cref="System.Resources.ResourceManager"/> singleton instance.
    /// </summary>
    [Fact]
    public void ResourceManager_ReturnsNonNullAndCachedInstance()
    {
        // Act
        var manager1 = ErrorResource.ResourceManager;
        var manager2 = ErrorResource.ResourceManager;

        // Assert
        Assert.NotNull(manager1);
        Assert.Same(manager1, manager2);
        Assert.Equal("Localization.Resources.ErrorResource", manager1.BaseName);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorResource.Culture"/> property can be set and retrieved.
    /// </summary>
    [Fact]
    public void Culture_SetAndGet_UpdatesCorrectly()
    {
        // Arrange
        var culture = new CultureInfo("it-IT");

        // Act
        ErrorResource.Culture = culture;

        // Assert
        Assert.Equal(culture, ErrorResource.Culture);

        // Reset
        ErrorResource.Culture = null;
        Assert.Null(ErrorResource.Culture);
    }

    /// <summary>
    /// Verifies that known localized error strings return non-empty text.
    /// </summary>
    [Fact]
    public void KnownProperties_ReturnExpectedValues()
    {
        // Act & Assert
        Assert.False(string.IsNullOrWhiteSpace(ErrorResource.Error));
        Assert.False(string.IsNullOrWhiteSpace(ErrorResource.Error404));
        Assert.False(string.IsNullOrWhiteSpace(ErrorResource.Error404Button));
        Assert.False(string.IsNullOrWhiteSpace(ErrorResource.Error404Title));
    }

    /// <summary>
    /// Verifies that all public static string properties of <see cref="ErrorResource"/>
    /// can be accessed without exception and return valid non-null strings.
    /// </summary>
    [Fact]
    public void AllProperties_ExecuteSuccessfullyAndReturnNonNullValues()
    {
        // Arrange
        PropertyInfo[] properties =
        [.. typeof(ErrorResource)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(string))];

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
