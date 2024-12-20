using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents an Uniform Resource Locator (URL). URLs are used to direct to documents, files, pictures, and many other Internet media.
/// See <see href="https://en.wikipedia.org/wiki/URL"/> and <see href="https://en.wikipedia.org/wiki/Uniform_Resource_Identifier"/> for more information.
/// </summary>
public record Url
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
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Url"/> class.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI) object.</param>
  public Url(Uri uri)
  {
    Uri = uri;
    Value = uri.ToString();
    new Validator().ValidateAndThrow(this);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="Url"/> class.
  /// </summary>
  /// <param name="value">A string representation of an Uniform Resource Identifier (URI).</param>
  public Url(string value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);

    Uri = new Uri(value, UriKind.Absolute);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="Url"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static Url? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the Url.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="Url"/>.
  /// </summary>
  private class Validator : AbstractValidator<Url>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).Url();
    }
  }
}
