using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class DisplayNameValidator : AbstractValidator<string>
{
  public DisplayNameValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(DisplayNameUnit.MaximumLength);
  }
}
