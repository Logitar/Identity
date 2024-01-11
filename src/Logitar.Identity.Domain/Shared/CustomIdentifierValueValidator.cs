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
  public CustomIdentifierValueValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(MaximumLength);
  }
}
