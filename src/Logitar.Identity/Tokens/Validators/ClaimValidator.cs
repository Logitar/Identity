using FluentValidation;

namespace Logitar.Identity.Tokens.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="Claim"/> class.
/// </summary>
internal class ClaimValidator : AbstractValidator<Claim>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ClaimValidator"/> class.
  /// </summary>
  public ClaimValidator()
  {
    RuleFor(x => x.Type).NotEmpty();

    RuleFor(x => x.Value).NotEmpty();

    RuleFor(x => x.ValueType).NullOrNotEmpty();
  }
}
