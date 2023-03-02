using FluentValidation;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ReadOnlyEmail"/> record.
/// </summary>
internal class ReadOnlyEmailValidator : AbstractValidator<ReadOnlyEmail>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyEmail"/> class.
  /// </summary>
  public ReadOnlyEmailValidator()
  {
    RuleFor(x => x.Address).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .EmailAddress();
  }
}
