using FluentValidation;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="PasswordSettings"/> record.
/// </summary>
internal class PasswordSettingsValidator : AbstractValidator<PasswordSettings>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PasswordSettingsValidator"/> class.
  /// </summary>
  public PasswordSettingsValidator()
  {
    RuleFor(x => x.RequiredLength).GreaterThanOrEqualTo(1);

    RuleFor(x => x.RequiredUniqueChars).GreaterThanOrEqualTo(1)
      .LessThanOrEqualTo(x => x.RequiredLength);
  }
}
