using FluentValidation;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Application.Account.Validators;

internal class RegisterPayloadValidator : AbstractValidator<RegisterPayload>
{
  public RegisterPayloadValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
  }
}
