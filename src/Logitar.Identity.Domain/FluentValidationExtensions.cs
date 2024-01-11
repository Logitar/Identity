using FluentValidation;
using NodaTime;

namespace Logitar.Identity.Domain;

/// <summary>
/// Extensions for FluentValidation.
/// </summary>
public static class FluentValidationExtensions
{
  /// <summary>
  /// Defines an allowed character validator on the current rule builder.
  /// Validation will fail if the value contains characters that are not allowed.
  /// Validation will succeed if the value only contains characters that are allowed.
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="allowedCharacters">The list of allowed characters.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilderOptions<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(s => allowedCharacters == null || s.All(allowedCharacters.Contains))
      .WithErrorCode("AllowedCharactersValidator")
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }

  /// <summary>
  /// Defines a future validator on the current rule builder.
  /// Validation will fail if the value is a date and time set in the past.
  /// Validation will succeed if the value is a date and time set in the future.
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="moment">The moment used to validate (defaults to now).</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInTheFuture(value, moment))
      .WithErrorCode("FutureValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the future.");
  }
  internal static bool BeInTheFuture(DateTime value, DateTime? moment) => value > (moment ?? DateTime.Now);

  private const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;
  public static IRuleBuilderOptions<T, string> Locale<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode("LocaleValidator")
      .WithMessage("'{PropertyName}' may not be the invariant culture, nor a user-defined culture.");
  }
  private static bool BeAValidLocale(string name)
  {
    try
    {
      CultureInfo culture = CultureInfo.GetCultureInfo(name);
      return !string.IsNullOrEmpty(culture.Name) && culture.LCID != LOCALE_CUSTOM_UNSPECIFIED;
    }
    catch (CultureNotFoundException)
    {
      return false;
    }
  }

  /// <summary>
  /// Defines a past validator on the current rule builder.
  /// Validation will fail if the value is a date and time set in the future.
  /// Validation will succeed if the value is a date and time set in the past.
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="moment">The moment used to validate (defaults to now).</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInThePast(value, moment))
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the past.");
  }
  internal static bool BeInThePast(DateTime value, DateTime? moment) => value < (moment ?? DateTime.Now);

  public static IRuleBuilderOptions<T, string> TimeZone<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(id => DateTimeZoneProviders.Tzdb.GetZoneOrNull(id) != null)
      .WithErrorCode("TimeZoneValidator")
      .WithMessage("'{PropertyName}' did not resolve to a tz entry.");
  }

  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(urlString => Uri.IsWellFormedUriString(urlString, UriKind.Absolute))
      .WithErrorCode("UrlValidator")
      .WithMessage("'{PropertyName}' must be a valid Uniform Resource Locator. See https://en.wikipedia.org/wiki/URL for more info.");
  }

  /// <summary>
  /// Sets the name of the validated property. This will set the value of the PropertyName property and in error messages.
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <typeparam name="TProperty">The type of the validated property.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="propertyName">The name of the validated property.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, TProperty> WithPropertyName<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder, string? propertyName)
  {
    return propertyName == null
      ? ruleBuilder
      : ruleBuilder.OverridePropertyName(propertyName).WithName(propertyName);
  }
}
