using FluentValidation;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Application.Account.Validators;

internal class RegisterPayloadValidator : AbstractValidator<RegisterPayload>
{
  public RegisterPayloadValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));

    When(x => x.EmailAddress != null, () => RuleFor(x => x.EmailAddress!).SetValidator(new EmailAddressValidator()));

    When(x => x.FirstName != null, () => RuleFor(x => x.FirstName!).SetValidator(new PersonNameValidator()));
    When(x => x.LastName != null, () => RuleFor(x => x.LastName!).SetValidator(new PersonNameValidator()));
  }
}
