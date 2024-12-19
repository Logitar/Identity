using FluentValidation;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core;

/// <summary>
/// Defines extension methods for input validation.
/// </summary>
public static class ValidationExtensions
{
  // TODO(fpion): Address

  /// <summary>
  /// Defines a 'allowed characters' validator on the current rule builder.
  /// Validation will fail if the property contains characters that is not allowed.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="allowedCharacters">The allowed characters.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.SetValidator(new AllowedCharactersValidator<T>(allowedCharacters));
  }

  // TODO(fpion): Birthdate

  // TODO(fpion): CustomAttributeKey

  // TODO(fpion): CustomAttributeValue

  /// <summary>
  /// Defines a 'description' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, or only white-space.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Description<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty();
  }

  /// <summary>
  /// Defines a 'display name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> DisplayName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.DisplayName.MaximumLength);
  }

  // TODO(fpion): Email

  /// <summary>
  /// Defines a 'future' validator on the current rule builder.
  /// Validation will fail if the property is not a date and time set in the future.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="now">The current moment (now) date and time reference.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new FutureValidator<T>(now));
  }

  /// <summary>
  /// Defines a 'gender' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Gender<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Users.Gender.MaximumLength);
  }

  /// <summary>
  /// Defines a 'identifier' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid identifier.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Identifier<T>(this IRuleBuilder<T, string> ruleBuilder) // TODO(fpion): ValueObject?
  {
    return ruleBuilder.NotEmpty().MaximumLength(byte.MaxValue).SetValidator(new IdentifierValidator<T>());
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified value is an identifier.
  /// An identifier only contains letters, digits and underscores (_) and cannot start with a digit.
  /// </summary>
  /// <param name="value">The input string.</param>
  /// <returns>True if the value is an identifier, or false otherwise.</returns>
  public static bool IsIdentifier(this string value)
  {
    return !string.IsNullOrEmpty(value) && !char.IsDigit(value.First()) && value.All(c => char.IsLetterOrDigit(c) || c == '_');
  }

  /// <summary>
  /// Defines a 'locale' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid locale.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Locale<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.Locale.MaximumLength).SetValidator(new LocaleValidator<T>());
  }

  // TODO(fpion): Password with Settings (such as UniqueName)

  /// <summary>
  /// Defines a 'past' validator on the current rule builder.
  /// Validation will fail if the property is not a date and time set in the past.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="now">The current moment (now) date and time reference.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new PastValidator<T>(now));
  }

  /// <summary>
  /// Defines a 'person name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> PersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Users.PersonName.MaximumLength);
  }

  // TODO(fpion): Phone

  /// <summary>
  /// Defines a 'time zone' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid tz database entry ID.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> TimeZone<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.TimeZone.MaximumLength).SetValidator(new TimeZoneValidator<T>());
  }

  /// <summary>
  /// Defines a 'unique name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or contains characters that are not allowed.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="uniqueNameSettings">The unique name settings.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder, IUniqueNameSettings uniqueNameSettings)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.UniqueName.MaximumLength).AllowedCharacters(uniqueNameSettings.AllowedCharacters);
  }

  /// <summary>
  /// Defines a 'url' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid absolute Uniform Resource Locator (URL).
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="schemes">The allowed URL schemes. Defaults to 'http' and 'https'.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<string>? schemes = null)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.Url.MaximumLength).SetValidator(new UrlValidator<T>(schemes));
  }
}
