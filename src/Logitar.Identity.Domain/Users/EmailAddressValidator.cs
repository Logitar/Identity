using FluentValidation;

namespace Logitar.Identity.Domain.Users;

public class EmailAddressValidator : AbstractValidator<string>
{
  public EmailAddressValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(EmailUnit.AddressMaximumLength).EmailAddress();
  }
}
