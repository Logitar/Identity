﻿using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents an Uniform Resource Locator (URL). URLs are used to direct to documents, files, pictures, and many other Internet media.
/// See <see href="https://en.wikipedia.org/wiki/URL"/> and <see href="https://en.wikipedia.org/wiki/Uniform_Resource_Identifier"/> for more information.
/// </summary>
public record UrlUnit
{
  /// <summary>
  /// The maximum length of Uniform Resource Locators (URL).
  /// </summary>
  public const int MaximumLength = 2048;
  /// <summary>
  /// The safe characters in Uniform Resource Locators (URL).
  /// </summary>
  public const string SafeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

  /// <summary>
  /// Gets the Uniform Resource Identifier (URI) object.
  /// </summary>
  public Uri Uri { get; }
  /// <summary>
  /// Gets a string representation of the Uniform Resource Locator (URL).
  /// </summary>
  public string Value => Uri.ToString();

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlUnit"/> class.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI) object.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UrlUnit(Uri uri, string? propertyName = null)
  {
    new UrlValidator(propertyName).ValidateAndThrow(uri.ToString());

    Uri = uri;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="UrlUnit"/> class.
  /// </summary>
  /// <param name="value">A string representation of an Uniform Resource Identifier (URI).</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UrlUnit(string value, string? propertyName = null)
  {
    value = value.Trim();
    new UrlValidator(propertyName).ValidateAndThrow(value);

    Uri = new(value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="UrlUnit"/> class otherwise.
  /// </summary>
  /// <param name="uriString">The string representation of the Uniform Resource Identifier (URI).</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static UrlUnit? TryCreate(string? uriString, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(uriString) ? null : new(uriString.Trim(), propertyName);
  }
}
