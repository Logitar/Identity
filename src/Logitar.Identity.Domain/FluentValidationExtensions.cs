using FluentValidation;

namespace Logitar.Identity.Domain;

/// <summary>
/// Extensions for FluentValidation.
/// </summary>
public static class FluentValidationExtensions
{
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
