using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to enforce that a string is an identifier.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
public class IdentifierValidator<T> : IPropertyValidator<T, string>
{
  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "IdentifierValidator";

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' may only contain letters, digits and underscores (_), and must not start with a digit.";
  }

  /// <summary>
  /// Validates a specific property value.
  /// </summary>
  /// <param name="context">The validation context.</param>
  /// <param name="value">The value to validate.</param>
  /// <returns>True if the value is valid, or false otherwise.</returns>
  public bool IsValid(ValidationContext<T> context, string value)
  {
    return string.IsNullOrEmpty(value) || (!char.IsDigit(value.First()) && value.All(c => char.IsLetterOrDigit(c) || c == '_'));
  }
}
