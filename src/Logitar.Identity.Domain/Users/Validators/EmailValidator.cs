using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class EmailValidator : AbstractValidator<IEmail>
{
  public EmailValidator()
  {
    RuleFor(x => x.Address).NotEmpty()
      .MaximumLength(EmailUnit.MaximumLength)
      .EmailAddress();
  }
}
