using FluentValidation;
using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="EmailUpdatedEvent"/> class.
/// </summary>
internal class EmailUpdatedValidator : AbstractValidator<EmailUpdatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailUpdatedValidator"/> class.
  /// </summary>
  public EmailUpdatedValidator() : base()
  {
    When(x => x.Email == null, () => RuleFor(x => x.EmailVerification).Equal(VerificationAction.None))
      .Otherwise(() => RuleFor(x => x.Email!).SetValidator(new ReadOnlyEmailValidator()));
  }
}
