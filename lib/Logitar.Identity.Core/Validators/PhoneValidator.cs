using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// Represents the validator for instances of <see cref="IPhone"/>.
/// </summary>
public class PhoneValidator : AbstractValidator<IPhone>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneValidator"/> class.
  /// </summary>
  public PhoneValidator()
  {
    When(x => x.CountryCode != null, () => RuleFor(x => x.CountryCode).NotEmpty().Length(Phone.CountryCodeMaximumLength));
    RuleFor(x => x.Number).NotEmpty().MaximumLength(Phone.NumberMaximumLength);
    When(x => x.Extension != null, () => RuleFor(x => x.Extension).NotEmpty().MaximumLength(Phone.ExtensionMaximumLength));

    RuleFor(x => x).Must(phone => phone.IsValid())
      .WithErrorCode("PhoneValidator")
      .WithMessage("'{PropertyName}' must be a valid phone number.");
  }
}
