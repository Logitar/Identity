using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public record DescriptionUnit
{
  public string Value { get; }

  public DescriptionUnit(string value)
  {
    Value = value.Trim();
    new DescriptionValidator().ValidateAndThrow(Value);
  }

  public static DescriptionUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
