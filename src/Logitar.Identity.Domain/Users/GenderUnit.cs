using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Security.Claims;

namespace Logitar.Identity.Domain.Users;

public record GenderUnit
{
  public const int MaximumLength = byte.MaxValue;

  public static readonly IEnumerable<string> KnownGenders = new[] { Genders.Female, Genders.Male };

  public string Value { get; }

  public GenderUnit(string value)
  {
    Value = Format(value);
    new GenderValidator().ValidateAndThrow(Value);
  }

  public static bool IsKnownGender(string value) => KnownGenders.Contains(value.ToLower());

  private static string Format(string value)
  {
    value = value.Trim();
    return IsKnownGender(value) ? value.ToLower() : value;
  }
}
