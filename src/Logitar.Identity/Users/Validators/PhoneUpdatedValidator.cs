using FluentValidation;
using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="PhoneUpdatedEvent"/> class.
/// </summary>
internal class PhoneUpdatedValidator : AbstractValidator<PhoneUpdatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneUpdatedValidator"/> class.
  /// </summary>
  public PhoneUpdatedValidator() : base()
  {
    When(x => x.Phone == null, () => RuleFor(x => x.PhoneVerification).Equal(VerificationAction.None))
      .Otherwise(() => RuleFor(x => x.Phone!).SetValidator(new ReadOnlyPhoneValidator()));
  }
}
