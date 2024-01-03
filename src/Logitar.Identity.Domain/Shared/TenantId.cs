using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public record TenantId
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public TenantId(string value)
  {
    Value = value.Trim();
    new TenantIdValidator().ValidateAndThrow(Value);
  }
}
