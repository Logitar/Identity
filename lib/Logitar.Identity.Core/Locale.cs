using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents a locale. A locale is composed of a required <see href="https://en.wikipedia.org/wiki/ISO_639-1">ISO 639-1 Alpha-2 language code</see>, followed
/// by an optional <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 Alpha-2 country code</see>. These codes shall be separated by an
/// hyphen (-).
/// </summary>
public record Locale
{
  /// <summary>
  /// The maximum length of locale codes.
  /// </summary>
  public const int MaximumLength = 16;

  /// <summary>
  /// Gets the culture of the locale.
  /// </summary>
  public CultureInfo Culture { get; }
  /// <summary>
  /// Gets the multipart code of the locale.
  /// </summary>
  public string Code { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Locale"/> class.
  /// </summary>
  /// <param name="culture">The culture object.</param>
  public Locale(CultureInfo culture)
  {
    Code = culture.Name;
    new Validator().ValidateAndThrow(this);

    Culture = culture;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="Locale"/> class.
  /// </summary>
  /// <param name="code">The multipart code of the locale.</param>
  public Locale(string code)
  {
    Code = code.Trim();
    new Validator().ValidateAndThrow(this);

    Culture = CultureInfo.GetCultureInfo(Code);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="Locale"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static Locale? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the locale.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{Culture.DisplayName} ({Culture.Name})";

  /// <summary>
  /// Represents the validator for instances of <see cref="Locale"/>.
  /// </summary>
  private class Validator : AbstractValidator<Locale>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Code).Locale();
    }
  }
}
