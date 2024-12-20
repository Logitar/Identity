using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// Represents the validator for instances of <see cref="IAddress"/>.
/// </summary>
public class AddressValidator : AbstractValidator<IAddress>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AddressValidator"/> class.
  /// </summary>
  /// <param name="helper">The address helper.</param>
  public AddressValidator(IAddressHelper helper)
  {
    RuleFor(x => x.Street).NotEmpty().MaximumLength(Address.MaximumLength);

    RuleFor(x => x.Locality).NotEmpty().MaximumLength(Address.MaximumLength);

    When(x => helper.GetCountry(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(Address.MaximumLength)
        .Matches(address => helper.GetCountry(address.Country)!.PostalCode).WithErrorCode("PostalCodeValidator"))
      .Otherwise(() => When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(Address.MaximumLength)));

    When(x => helper.GetCountry(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty().MaximumLength(Address.MaximumLength)
        .Must((address, region) => helper.GetCountry(address.Country)!.Regions!.Contains(region)).WithErrorCode("RegionValidator")
          .WithMessage(address => $"'{{PropertyName}}' must be one of the following: {string.Join(", ", helper.GetCountry(address.Country)!.Regions!)}."))
      .Otherwise(() => When(x => x.Region != null, () => RuleFor(x => x.Region).NotEmpty().MaximumLength(Address.MaximumLength)));

    RuleFor(x => x.Country).NotEmpty().MaximumLength(Address.MaximumLength)
      .Must(helper.IsSupported).WithErrorCode("CountryValidator").WithMessage($"'{{PropertyName}}' must be one of the following: {string.Join(", ", helper.SupportedCountries)}.");
  }
}
