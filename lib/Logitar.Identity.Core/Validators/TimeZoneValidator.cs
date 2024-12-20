using FluentValidation;
using FluentValidation.Validators;
using NodaTime;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to validate time zones.
/// See <see cref="TimeZone"/> for more information.
/// </summary>
public class TimeZoneValidator<T> : IPropertyValidator<T, string>
{
  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "TimeZoneValidator";

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must correspond to a valid tz database entry ID.";
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
      DateTimeZone? dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(value);
      return dateTimeZone != null;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
