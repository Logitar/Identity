using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record PersonNameUnit
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public PersonNameUnit(string value)
  {
    Value = value.Trim();
    new PersonNameValidator().ValidateAndThrow(Value);
  }

  public static PersonNameUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
