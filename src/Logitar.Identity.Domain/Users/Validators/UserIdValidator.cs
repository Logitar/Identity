using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class UserIdValidator : AbstractValidator<string>
{
  public UserIdValidator(int maximumLength)
  {
    RuleFor(x => x).NotEmpty().MaximumLength(maximumLength);
  }
}
