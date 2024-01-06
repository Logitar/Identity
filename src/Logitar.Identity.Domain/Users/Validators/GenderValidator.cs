using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class GenderValidator : AbstractValidator<string>
{
  public GenderValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(GenderUnit.MaximumLength);
  }
}
