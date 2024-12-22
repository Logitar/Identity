using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to enforce that a string is a valid locale code. See <see cref="Locale"/> for more information.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
public class LocaleValidator<T> : IPropertyValidator<T, string>
{
  /// <summary>
  /// The LCID used for user-defined cultures.
  /// </summary>
  private const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;

  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "LocaleValidator";

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a valid locale code. It cannot be the invariant culture, nor an user-defined culture.";
  }

  /// <summary>
  /// Validates a specific property value.
  /// </summary>
  /// <param name="context">The validation context.</param>
  /// <param name="value">The value to validate.</param>
  /// <returns>True if the value is valid, or false otherwise.</returns>
  public bool IsValid(ValidationContext<T> context, string value)
  {
    try
    {
      CultureInfo culture = CultureInfo.GetCultureInfo(value);
      return !string.IsNullOrWhiteSpace(culture.Name) && culture.LCID != LOCALE_CUSTOM_UNSPECIFIED;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
