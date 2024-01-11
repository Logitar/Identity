using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom identifier keys.
/// See <see cref="CustomIdentifierValidator"/> for more information.
/// </summary>
public class CustomIdentifierKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierKeyValidator"/> class.
  /// </summary>
  public CustomIdentifierKeyValidator()
  {
    RuleFor(x => x).SetValidator(new IdentifierValidator());
  }
}
