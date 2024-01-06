using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class LocaleValidator : AbstractValidator<string>
{
  public LocaleValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(LocaleUnit.MaximumLength)
      .Locale();
  }
}
