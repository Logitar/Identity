using FluentValidation;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Shared;

public record UniqueNameUnit
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public UniqueNameUnit(IUniqueNameSettings uniqueNameSettings, string value)
  {
    Value = value.Trim();
    new UniqueNameValidator(uniqueNameSettings).ValidateAndThrow(Value);
  }

  public static UniqueNameUnit? TryCreate(IUniqueNameSettings uniqueNameSettings, string? value) => string.IsNullOrWhiteSpace(value) ? null : new(uniqueNameSettings, value);
}
