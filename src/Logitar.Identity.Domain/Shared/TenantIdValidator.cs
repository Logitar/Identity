using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class TenantIdValidator : AbstractValidator<string>
{
  public TenantIdValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(TenantId.MaximumLength)
      .AllowedCharacters(TenantId.UriSafeCharacters);
  }
}
