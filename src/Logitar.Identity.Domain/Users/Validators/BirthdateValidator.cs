using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class BirthdateValidator : AbstractValidator<DateTime>
{
  public BirthdateValidator()
  {
    RuleFor(x => x).Past();
  }
}
