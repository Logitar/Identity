using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom attribute keys.
/// See <see cref="CustomAttributeValidator"/> for more information.
/// </summary>
public class CustomAttributeKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CustomAttributeKeyValidator"/> class.
  /// </summary>
  public CustomAttributeKeyValidator()
  {
    RuleFor(x => x).SetValidator(new IdentifierValidator());
  }
}
