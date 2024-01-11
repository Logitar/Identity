using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

/// <summary>
/// The validator used to validate postal addresses.
/// See <see cref="AddressUnit"/> for more information.
/// </summary>
public class AddressValidator : AbstractValidator<IAddress>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AddressValidator"/> class.
  /// </summary>
  public AddressValidator()
  {
    RuleFor(x => x.Street).NotEmpty().MaximumLength(AddressUnit.MaximumLength);

    RuleFor(x => x.Locality).NotEmpty().MaximumLength(AddressUnit.MaximumLength);

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(AddressUnit.MaximumLength)
      .Must(PostalAddressHelper.IsSupported)
        .WithErrorCode("CountryValidator")
        .WithMessage($"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.SupportedCountries)}");

    When(x => PostalAddressHelper.GetCountry(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .Must((address, region) => PostalAddressHelper.GetCountry(address.Country)?.Regions?.Contains(region) == true)
          .WithErrorCode("RegionValidator")
          .WithMessage($"'{{PropertyName}}'")
    ).Otherwise(() =>
      When(x => x.Region != null, () => RuleFor(x => x.Region).NotEmpty().MaximumLength(AddressUnit.MaximumLength))
    );

    When(x => PostalAddressHelper.GetCountry(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .Matches(address => PostalAddressHelper.GetCountry(address.Country)?.PostalCode).WithErrorCode("PostalCodeValidator")
    ).Otherwise(() =>
      When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(AddressUnit.MaximumLength))
    );
  }
}
