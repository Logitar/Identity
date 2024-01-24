using FluentValidation;
using Logitar.Identity.Contracts.Users;

namespace Logitar.Identity.Domain.Users.Validators;

/// <summary>
/// The validator used to validate phone numbers.
/// See <see cref="PhoneUnit"/> for more information.
/// </summary>
public class PhoneValidator : AbstractValidator<IPhone>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public PhoneValidator(string? propertyName = null)
  {
    When(x => x.CountryCode != null,
      () => RuleFor(x => x.CountryCode).NotEmpty()
        .Length(PhoneUnit.CountryCodeMaximumLength)
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IPhone.CountryCode)}")
    );

    RuleFor(x => x.Number).NotEmpty()
      .MaximumLength(PhoneUnit.NumberMaximumLength)
      .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IPhone.Number)}");

    When(x => x.Extension != null,
      () => RuleFor(x => x.Extension).NotEmpty()
        .MaximumLength(PhoneUnit.ExtensionMaximumLength)
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IPhone.Extension)}")
    );

    RuleFor(x => x).Must(phone => phone.IsValid())
      .WithErrorCode(nameof(PhoneValidator))
      .WithMessage("'{PropertyName}' must be a valid phone.")
      .WithPropertyName(propertyName);
  }
}
