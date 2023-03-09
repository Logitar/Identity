using FluentValidation;

namespace Logitar.Identity.Tokens.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="CreateTokenInput"/> class.
/// </summary>
internal class CreateTokenValidator : AbstractValidator<CreateTokenInput>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CreateTokenValidator"/> class.
  /// </summary>
  public CreateTokenValidator()
  {
    RuleFor(x => x.Lifetime).GreaterThan(0);

    RuleFor(x => x.Purpose).Purpose();

    RuleForEach(x => x.Claims).SetValidator(new ClaimValidator());
  }
}
