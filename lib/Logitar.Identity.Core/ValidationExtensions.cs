using FluentValidation;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core;

/// <summary>
/// Defines extension methods for input validation.
/// </summary>
public static class ValidationExtensions
{
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
  /// Defines a 'unique name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="uniqueNameSettings">The unique name settings.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder, IUniqueNameSettings uniqueNameSettings)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.UniqueName.MaximumLength).AllowedCharacters(uniqueNameSettings.AllowedCharacters);
  }
}
