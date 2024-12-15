using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate past dates.
/// </summary>
public class PastValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PastValidator"/> class.
  /// </summary>
  /// <param name="moment">The validation moment, defauts to now.</param>
  public PastValidator(DateTime? moment = null)
  {
    RuleFor(x => x).Must(value => value < (moment ?? DateTime.Now))
      .WithErrorCode(nameof(PastValidator))
      .WithMessage("'{PropertyName}' must be a date and time set in the past.");
  }
}
