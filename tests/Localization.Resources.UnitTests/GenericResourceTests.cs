// <copyright file="GenericResourceTests.cs" company="Localization.Resources Contributors">
// Copyright (c) Localization.Resources Contributors. All rights reserved.
// </copyright>

using System.Globalization;
using System.Reflection;

namespace Localization.Resources.UnitTests;

/// <summary>
/// Unit test suite for <see cref="GenericResource"/>.
/// </summary>
[Collection("CultureTests")]
public sealed class GenericResourceTests : IDisposable
{
    private readonly CultureInfo? _originalCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericResourceTests"/> class,
    /// preserving the initial resource culture.
    /// </summary>
    public GenericResourceTests()
    {
        _originalCulture = GenericResource.Culture;
    }

    /// <summary>
    /// Restores the original resource culture after test execution.
    /// </summary>
    public void Dispose() => GenericResource.Culture = _originalCulture;

    /// <summary>
    /// Verifies that the internal constructor initializes a non-null instance.
    /// </summary>
    [Fact]
    public void Constructor_InitializesInstanceSuccessfully()
    {
        // Act
        var instance = new GenericResource();

        // Assert
        Assert.NotNull(instance);
    }

    /// <summary>
    /// Verifies that <see cref="GenericResource.ResourceManager"/> returns a non-null,
    /// correctly named <see cref="System.Resources.ResourceManager"/> singleton instance.
    /// </summary>
    [Fact]
    public void ResourceManager_ReturnsNonNullAndCachedInstance()
    {
        // Act
        var manager1 = GenericResource.ResourceManager;
        var manager2 = GenericResource.ResourceManager;

        // Assert
        Assert.NotNull(manager1);
        Assert.Same(manager1, manager2);
        Assert.Equal("Localization.Resources.GenericResource", manager1.BaseName);
    }

    /// <summary>
    /// Verifies that <see cref="GenericResource.Culture"/> property can be set and retrieved.
    /// </summary>
    [Fact]
    public void Culture_SetAndGet_UpdatesCorrectly()
    {
        // Arrange
        var culture = new CultureInfo("de-DE");

        // Act
        GenericResource.Culture = culture;

        // Assert
        Assert.Equal(culture, GenericResource.Culture);

        // Reset
        GenericResource.Culture = null;
        Assert.Null(GenericResource.Culture);
    }

    /// <summary>
    /// Verifies that known localized generic strings return non-empty text.
    /// </summary>
    [Fact]
    public void KnownProperties_ReturnExpectedValues()
    {
        // Act & Assert
        Assert.False(string.IsNullOrWhiteSpace(GenericResource.Account));
        Assert.False(string.IsNullOrWhiteSpace(GenericResource.ActiveShiftAlreadyExists));
        Assert.False(string.IsNullOrWhiteSpace(GenericResource.AddAuthenticatorApp));
        Assert.False(string.IsNullOrWhiteSpace(GenericResource.AddEmail));
    }

    /// <summary>
    /// Verifies that all public static string properties of <see cref="GenericResource"/>
    /// can be accessed without exception and return valid non-null strings.
    /// </summary>
    [Fact]
    public void AllProperties_ExecuteSuccessfullyAndReturnNonNullValues()
    {
        // Arrange
        PropertyInfo[] properties =
        [
            .. typeof(GenericResource)
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
