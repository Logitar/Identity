using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Describes custom identifier validators.
/// </summary>
public interface ICustomIdentifierValidator
{
  /// <summary>
  /// Gets the custom identifier key validator.
  /// </summary>
  IValidator<string> KeyValidator { get; }
  /// <summary>
  /// Gets the custom identifier value validator.
  /// </summary>
  IValidator<string> ValueValidator { get; }

  /// <summary>
  /// Validates the specified custom identifier key and value.
  /// An <see cref="ValidationException"/> will be thrown if validation fails.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  void ValidateAndThrow(string key, string value);
}
