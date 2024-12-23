using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to enforce a strict list of characters.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
public class AllowedCharactersValidator<T> : IPropertyValidator<T, string>
{
  /// <summary>
  /// Gets the allowed characters.
  /// </summary>
  public string? AllowedCharacters { get; }
  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "AllowedCharactersValidator";

  /// <summary>
  /// Initializes a new instance of the <see cref="AllowedCharactersValidator{T}"/> class.
  /// </summary>
  /// <param name="allowedCharacters">The allowed characters.</param>
  public AllowedCharactersValidator(string? allowedCharacters)
  {
    AllowedCharacters = allowedCharacters;
  }

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return $"'{{PropertyName}}' may only include the following characters: {AllowedCharacters}";
  }

  /// <summary>
  /// Validates a specific property value.
  /// </summary>
  /// <param name="context">The validation context.</param>
  /// <param name="value">The value to validate.</param>
  /// <returns>True if the value is valid, or false otherwise.</returns>
  public bool IsValid(ValidationContext<T> context, string value)
  {
    return AllowedCharacters == null || value.All(AllowedCharacters.Contains);
  }
}
