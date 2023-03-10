using FluentValidation;

namespace Logitar.Identity.Tokens.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ValidateTokenInput"/> class.
/// </summary>
internal class ValidateTokenValidator : AbstractValidator<ValidateTokenInput>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateTokenValidator"/> class.
  /// </summary>
  public ValidateTokenValidator()
  {
    RuleFor(x => x.Token).NotEmpty();

    RuleFor(x => x.Purpose).Purpose();

    RuleFor(x => x.Realm).NullOrNotEmpty();
    When(x => x.Realm == null, () => RuleFor(x => x.Secret).NotEmpty()
      .MinimumLength(256 / 8)
      .MaximumLength(512 / 8));

    RuleFor(x => x.Audience).NullOrNotEmpty();

    RuleFor(x => x.Issuer).NullOrNotEmpty();
  }
}
