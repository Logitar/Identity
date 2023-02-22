using FluentValidation;
using Logitar.Identity.Realms.Events;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="RealmSavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal class RealmSavedValidator<T> : AbstractValidator<T> where T : RealmSavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmSavedValidator{T}"/> class.
  /// </summary>
  public RealmSavedValidator()
  {
    RuleFor(x => x.DisplayName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Description).NullOrNotEmpty();

    RuleFor(x => x.DefaultLocale).Locale();

    RuleFor(x => x.Url).NullOrNotEmpty()
      .MaximumLength(2048)
      .Url();

    RuleFor(x => x.UsernameSettings).SetValidator(new UsernameSettingsValidator());

    RuleFor(x => x.PasswordSettings).SetValidator(new PasswordSettingsValidator());

    RuleFor(x => x.JwtSecret).NotEmpty()
      .MinimumLength(256 / 8);

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();
  }
}
