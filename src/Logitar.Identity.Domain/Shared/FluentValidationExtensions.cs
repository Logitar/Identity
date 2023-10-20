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
  /// Defines a Uniform Resource Locator (URL) validator on the current rule builder.
  /// Validation will fail if the value is not a valid Uniform Resource Locator (URL).
  /// Validation will succeed if the value is a valid Uniform Resource Locator (URL).
  /// </summary>
  /// <typeparam name="T">The type of the validated object.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidUrl)
      .WithErrorCode("UrlValidator")
      .WithMessage("'{PropertyName}' must be a valid Uniform Resource Locator (URL).");
  }
  internal static bool BeAValidUrl(string url)
  {
    try
    {
      _ = new Uri(url);
      return true;
    }
    catch (Exception)
    {
      return false;
    }
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
