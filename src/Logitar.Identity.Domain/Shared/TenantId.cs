using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public record TenantId
{
  public string Value { get; }

  public TenantId(string value)
  {
    Value = value.Trim();
    new IdValidator().ValidateAndThrow(Value);
  }

  public static TenantId? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
