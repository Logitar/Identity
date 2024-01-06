using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class UrlValidator : AbstractValidator<string>
{
  public UrlValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(UrlUnit.MaximumLength)
      .AllowedCharacters(UrlUnit.SafeCharacters)
      .Url();
  }
}
