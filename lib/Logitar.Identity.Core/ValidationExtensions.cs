using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Defines extension methods for input validation.
/// </summary>
public static class ValidationExtensions
{
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
  /// Defines a 'unique name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.UniqueName.MaximumLength); // TODO(fpion): AllowedCharacters
  }
}
