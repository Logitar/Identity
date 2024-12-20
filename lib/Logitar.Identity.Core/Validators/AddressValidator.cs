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
  public AddressValidator()
  {
    RuleFor(x => x.Street).NotEmpty().MaximumLength(Address.MaximumLength);
    RuleFor(x => x.Locality).NotEmpty().MaximumLength(Address.MaximumLength);
    When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(Address.MaximumLength)); // TODO(fpion): validate from CountrySettings
    When(x => x.Region != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(Address.MaximumLength)); // TODO(fpion): validate from CountrySettings
    RuleFor(x => x.Country).NotEmpty().MaximumLength(Address.MaximumLength); // TODO(fpion): validate from CountrySettings
  }
}
