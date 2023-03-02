using FluentValidation;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ReadOnlyAddress"/> record.
/// </summary>
internal class ReadOnlyAddressValidator : AbstractValidator<ReadOnlyAddress>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyAddress"/> class.
  /// </summary>
  public ReadOnlyAddressValidator()
  {
    RuleFor(x => x.Line1).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Line2).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Country();

    RuleFor(x => x.Region).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    When(x => PostalAddressHelper.GetCountry(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Matches(x => PostalAddressHelper.GetCountry(x.Country)!.PostalCode))
      .Otherwise(() => RuleFor(x => x.PostalCode).NullOrNotEmpty()
        .MaximumLength(byte.MaxValue));

    When(x => PostalAddressHelper.GetCountry(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Must((x, r) => PostalAddressHelper.GetCountry(x.Country)!.Regions!.Contains(r)))
      .Otherwise(() => RuleFor(x => x.Region).NullOrNotEmpty()
        .MaximumLength(byte.MaxValue));
  }
}
