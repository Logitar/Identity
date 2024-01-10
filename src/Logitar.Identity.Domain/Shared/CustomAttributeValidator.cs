using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom attributes.
/// </summary>
public class CustomAttributeValidator : ICustomAttributeValidator
{
  /// <summary>
  /// Gets the custom attribute key validator.
  /// </summary>
  public IValidator<string> KeyValidator { get; }
  /// <summary>
  /// Gets the custom attribute value validator.
  /// </summary>
  public IValidator<string> ValueValidator { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomAttributeValidator"/> class.
  /// </summary>
  /// <param name="keyValidator">The custom attribute key validator.</param>
  /// <param name="valueValidator">The custom attribute value validator.</param>
  public CustomAttributeValidator(IValidator<string>? keyValidator = null, IValidator<string>? valueValidator = null)
  {
    KeyValidator = keyValidator ?? new CustomAttributeKeyValidator();
    ValueValidator = valueValidator ?? new CustomAttributeValueValidator();
  }

  /// <summary>
  /// Validates the specified custom attribute key and value.
  /// An <see cref="ValidationException"/> will be thrown if validation fails.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void ValidateAndThrow(string key, string value)
  {
    KeyValidator.ValidateAndThrow(key);
    ValueValidator.ValidateAndThrow(value);
  }
}
