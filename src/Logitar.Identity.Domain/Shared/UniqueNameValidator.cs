using FluentValidation;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Shared;

public class UniqueNameValidator : AbstractValidator<string>
{
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(UniqueNameUnit.MaximumLength)
      .AllowedCharacters(uniqueNameSettings.AllowedCharacters);
  }
}
