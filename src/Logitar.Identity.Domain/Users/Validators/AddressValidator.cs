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
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public AddressValidator(string? propertyName = null)
  {
    RuleFor(x => x.Street).NotEmpty()
      .MaximumLength(AddressUnit.MaximumLength)
      .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.Street)}");

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(AddressUnit.MaximumLength)
      .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.Locality)}");

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(AddressUnit.MaximumLength)
      .Must(PostalAddressHelper.IsSupported)
        .WithErrorCode("CountryValidator")
        .WithMessage($"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.SupportedCountries)}")
      .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.Country)}");

    When(x => PostalAddressHelper.GetCountry(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .Must((address, region) => PostalAddressHelper.GetCountry(address.Country)?.Regions?.Contains(region) == true)
          .WithErrorCode("RegionValidator")
          .WithMessage(address => $"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.GetCountry(address.Country)?.Regions ?? [])}")
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.Region)}")
    ).Otherwise(() =>
      When(x => x.Region != null, () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.Region)}")
      )
    );

    When(x => PostalAddressHelper.GetCountry(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .Matches(address => PostalAddressHelper.GetCountry(address.Country)?.PostalCode)
          .WithErrorCode("PostalCodeValidator")
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.PostalCode)}")
    ).Otherwise(() =>
      When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(AddressUnit.MaximumLength)
        .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IAddress.PostalCode)}")
      )
    );
  }
}
