using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class TimeZoneValidator : AbstractValidator<string>
{
  public TimeZoneValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(TimeZoneUnit.MaximumLength)
      .TimeZone();
  }
}
