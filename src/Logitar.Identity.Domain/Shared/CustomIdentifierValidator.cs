using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom identifiers.
/// </summary>
public class CustomIdentifierValidator : ICustomIdentifierValidator
{
  /// <summary>
  /// Gets the custom identifier key validator.
  /// </summary>
  public IValidator<string> KeyValidator { get; }
  /// <summary>
  /// Gets the custom identifier value validator.
  /// </summary>
  public IValidator<string> ValueValidator { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierValidator"/> class.
  /// </summary>
  /// <param name="keyValidator">The custom identifier key validator.</param>
  /// <param name="valueValidator">The custom identifier value validator.</param>
  public CustomIdentifierValidator(IValidator<string>? keyValidator = null, IValidator<string>? valueValidator = null)
  {
    KeyValidator = keyValidator ?? new CustomIdentifierKeyValidator();
    ValueValidator = valueValidator ?? new CustomIdentifierValueValidator();
  }

  /// <summary>
  /// Validates the specified custom identifier key and value.
  /// An <see cref="ValidationException"/> will be thrown if validation fails.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  public void ValidateAndThrow(string key, string value)
  {
    KeyValidator.ValidateAndThrow(key);
    ValueValidator.ValidateAndThrow(value);
  }
}
