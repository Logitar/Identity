using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents a locale. A locale is composed of a required <see href="https://en.wikipedia.org/wiki/ISO_639-1">ISO 639-1 Alpha-2 language code</see>, followed
/// by an optional <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 Alpha-2 country code</see>. These codes shall be separated by an
/// hyphen (-).
/// </summary>
public record LocaleUnit
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
  public string Code => Culture.Name;

  /// <summary>
  /// Initializes a new instance of the <see cref="LocaleUnit"/> class.
  /// </summary>
  /// <param name="culture">The culture object.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public LocaleUnit(CultureInfo culture, string? propertyName = null)
  {
    new LocaleValidator(propertyName).ValidateAndThrow(culture.Name);

    Culture = culture;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="LocaleUnit"/> class.
  /// </summary>
  /// <param name="code">The multipart code of the locale.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public LocaleUnit(string code, string? propertyName = null)
  {
    code = code.Trim();
    new LocaleValidator(propertyName).ValidateAndThrow(code);

    Culture = CultureInfo.GetCultureInfo(code.Trim());
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="LocaleUnit"/> class otherwise.
  /// </summary>
  /// <param name="code">The multipart code of the locale.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static LocaleUnit? TryCreate(string? code, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(code) ? null : new(code, propertyName);
  }
}
