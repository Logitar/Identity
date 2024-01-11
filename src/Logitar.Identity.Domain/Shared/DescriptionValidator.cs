using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class DescriptionValidator : AbstractValidator<string>
{
  public DescriptionValidator()
  {
    RuleFor(x => x).NotEmpty();
  }
}
