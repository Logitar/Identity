using FluentValidation;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Shared;

public class IdValidator : AbstractValidator<string>
{
  public IdValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(AggregateId.MaximumLength)
      .AllowedCharacters(UrlUnit.SafeCharacters);
  }
}
