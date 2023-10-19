using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents an abstraction of a custom attribute validator.
/// </summary>
public interface ICustomAttributeValidator
{
  /// <summary>
  /// Gets the custom attribute key validator.
  /// </summary>
  IValidator<string> KeyValidator { get; }
  /// <summary>
  /// Gets the custom attribute value validator.
  /// </summary>
  IValidator<string> ValueValidator { get; }

  /// <summary>
  /// Validates the specified custom attribute key and value.
  /// An <see cref="ValidationException"/> will be thrown if validation fails.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  void ValidateAndThrow(string key, string value);
}
