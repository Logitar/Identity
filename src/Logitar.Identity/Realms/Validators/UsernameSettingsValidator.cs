using FluentValidation;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="UsernameSettings"/> record.
/// </summary>
internal class UsernameSettingsValidator : AbstractValidator<UsernameSettings>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UsernameSettingsValidator"/> class.
  /// </summary>
  public UsernameSettingsValidator()
  {
    RuleFor(x => x.AllowedCharacters).NullOrNotEmpty();
  }
}
