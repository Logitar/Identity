using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class PersonNameValidator : AbstractValidator<string>
{
  public PersonNameValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(PersonNameUnit.MaximumLength);
  }
}
