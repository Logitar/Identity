using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate future dates.
/// </summary>
public class FutureValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="FutureValidator"/> class.
  /// </summary>
  /// <param name="moment">The validation moment, defauts to now.</param>
  public FutureValidator(DateTime? moment = null)
  {
    RuleFor(x => x).Must(value => value > (moment ?? DateTime.Now))
      .WithErrorCode(nameof(FutureValidator))
      .WithMessage("'{PropertyName}' must be a date and time set in the future.");
  }
}
