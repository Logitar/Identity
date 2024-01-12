using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom identifier values.
/// See <see cref="CustomIdentifierValidator"/> for more information.
/// </summary>
public class CustomIdentifierValueValidator : AbstractValidator<string>
{
  /// <summary>
  /// The maximum length of custom identifier values.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierValueValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public CustomIdentifierValueValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty().MaximumLength(MaximumLength).WithPropertyName(propertyName);
  }
}
