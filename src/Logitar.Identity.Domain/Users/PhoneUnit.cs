using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record PhoneUnit : ContactUnit, IPhone
{
  public const int CountryCodeLength = 2;
  public const int NumberMaximumLength = 17;
  public const int ExtensionMaximumLength = 10;

  public string? CountryCode { get; }
  public string Number { get; }
  public string? Extension { get; }

  public PhoneUnit(string number, string? countryCode = null, string? extension = null, bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
    new PhoneValidator().ValidateAndThrow(this);
  }

  public static PhoneUnit? TryCreate(string? number, string? countryCode = null, string? extension = null, bool isVerified = false)
    => string.IsNullOrWhiteSpace(number) ? null : new(number, countryCode, extension, isVerified);
}
