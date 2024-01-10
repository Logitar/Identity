using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class PhoneValidator : AbstractValidator<IPhone>
{
  public PhoneValidator()
  {
    When(x => x.CountryCode != null, () => RuleFor(x => x.CountryCode).NotEmpty().Length(PhoneUnit.CountryCodeLength));
    RuleFor(x => x.Number).NotEmpty().MaximumLength(PhoneUnit.NumberMaximumLength);
    When(x => x.Extension != null, () => RuleFor(x => x.Extension).NotEmpty().Length(PhoneUnit.ExtensionMaximumLength));

    RuleFor(x => x).Must(phone => phone.IsValid())
      .WithErrorCode(nameof(PhoneValidator))
      .WithMessage("'{PropertyName}' must be a valid phone.");
  }
}
