using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class TenantIdValidator : AbstractValidator<string>
{
  private const string UriSafeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

  public TenantIdValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(TenantId.MaximumLength)
      .AllowedCharacters(UriSafeCharacters);
  }
}
