using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

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
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: '{allowedCharacters}'.");
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
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilderOptions<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInTheFuture(value, moment))
      .WithErrorCode("FutureValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the future.");
  }
  internal static bool BeInTheFuture(DateTime value, DateTime? moment)
  {
    return value > (moment ?? DateTime.Now);
  }

  /// <summary>
  /// Defines an identifier validator on the current rule builder.
  /// Validation will fail if the value starts with a digit or contains characters that are not letters, digits or underscores (_).
  /// Validation will succeed if the value does not start with a digit and only contains letters, digits and underscores (_).
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string> Identifier<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidIdentifier)
      .WithErrorCode("IdentifierValidator")
      .WithMessage("'{PropertyName}' may not start with a digit and may only contain letters, digits and underscores (_).");
  }
  internal static bool BeAValidIdentifier(string identifier)
  {
    return !char.IsDigit(identifier.FirstOrDefault()) && identifier.All(c => char.IsLetterOrDigit(c) || c == '_');
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
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilderOptions<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInThePast(value, moment))
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the past.");
  }
  internal static bool BeInThePast(DateTime value, DateTime? moment)
  {
    return value < (moment ?? DateTime.Now);
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
