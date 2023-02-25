using FluentValidation;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ReadOnlyPhone"/> record.
/// </summary>
internal class ReadOnlyPhoneValidator : AbstractValidator<ReadOnlyPhone>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPhone"/> class.
  /// </summary>
  public ReadOnlyPhoneValidator()
  {
    RuleFor(x => x.CountryCode).NullOrNotEmpty()
      .MaximumLength(16);

    RuleFor(x => x.Number).NotEmpty()
      .MaximumLength(32);

    RuleFor(x => x.Extension).NullOrNotEmpty()
      .MaximumLength(16);

    RuleFor(x => x).PhoneNumber();
  }
}
