using FluentValidation;
using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="UserSavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal class UserSavedValidator<T> : AbstractValidator<T> where T : UserSavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserSavedValidator{T}"/> class.
  /// </summary>
  protected UserSavedValidator()
  {
    RuleFor(x => x.PasswordHash).NullOrNotEmpty();

    When(x => x.Address == null, () => RuleFor(x => x.AddressVerification).Equal(VerificationAction.None))
      .Otherwise(() => RuleFor(x => x.Address!).SetValidator(new ReadOnlyAddressValidator()));
    When(x => x.Email == null, () => RuleFor(x => x.EmailVerification).Equal(VerificationAction.None))
      .Otherwise(() => RuleFor(x => x.Email!).SetValidator(new ReadOnlyEmailValidator()));
    When(x => x.Phone == null, () => RuleFor(x => x.PhoneVerification).Equal(VerificationAction.None))
      .Otherwise(() => RuleFor(x => x.Phone!).SetValidator(new ReadOnlyPhoneValidator()));

    RuleFor(x => x.FirstName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);
    RuleFor(x => x.MiddleName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);
    RuleFor(x => x.LastName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);
    RuleFor(x => x.FullName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue * 3 + 2);
    RuleFor(x => x.Nickname).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locale).Locale();

    RuleFor(x => x.TimeZone).NullOrNotEmpty()
      .TimeZone();

    RuleFor(x => x.Picture).NullOrNotEmpty()
      .MaximumLength(2048)
      .Url();

    RuleFor(x => x.Profile).NullOrNotEmpty()
      .MaximumLength(2048)
      .Url();

    RuleFor(x => x.Website).NullOrNotEmpty()
      .MaximumLength(2048)
      .Url();

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();
  }
}
