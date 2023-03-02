using FluentValidation;
using Logitar.Identity.Realms.Events;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="RealmSavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal abstract class RealmSavedValidator<T> : AbstractValidator<T> where T : RealmSavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmSavedValidator{T}"/> class.
  /// </summary>
  protected RealmSavedValidator()
  {
    RuleFor(x => x.DisplayName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Description).NullOrNotEmpty();

    RuleFor(x => x.DefaultLocale).Locale();

    RuleFor(x => x.Url).NullOrNotEmpty()
      .MaximumLength(2048)
      .Url();

    RuleFor(x => x.UsernameSettings).SetValidator(new ReadOnlyUsernameSettingsValidator());

    RuleFor(x => x.PasswordSettings).SetValidator(new ReadOnlyPasswordSettingsValidator());

    RuleFor(x => x.JwtSecret).NotEmpty()
      .MinimumLength(256 / 8)
      .MaximumLength(512 / 8);

    RuleForEach(x => x.ClaimMappings.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.ClaimMappings.Values).SetValidator(new ReadOnlyClaimMappingValidator());

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();

    When(x => x.GoogleOAuth2Configuration != null,
      () => RuleFor(x => x.GoogleOAuth2Configuration!).SetValidator(new ReadOnlyGoogleOAuth2ConfigurationValidator()));
  }
}
