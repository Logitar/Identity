using FluentValidation;

namespace Logitar.Identity.Domain.ApiKeys.Validators;

public class ExpirationValidator : AbstractValidator<DateTime>
{
  public ExpirationValidator()
  {
    RuleFor(x => x).Future();
  }
}
