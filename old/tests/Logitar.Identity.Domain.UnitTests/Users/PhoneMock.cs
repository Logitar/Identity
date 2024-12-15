using Logitar.Identity.Contracts.Users;

namespace Logitar.Identity.Domain.Users;

internal record PhoneMock : IPhone
{
  public string? CountryCode { get; }
  public string Number { get; }
  public string? Extension { get; }

  public PhoneMock(string number = "+15148454636", string? countryCode = "CA", string? extension = "12345")
  {
    CountryCode = countryCode;
    Number = number;
    Extension = extension;
  }
}
