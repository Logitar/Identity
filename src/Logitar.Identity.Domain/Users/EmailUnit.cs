using FluentValidation;

namespace Logitar.Identity.Domain.Users;

public record EmailUnit : ContactUnit
{
  public const int AddressMaximumLength = byte.MaxValue;

  public string Address { get; }

  public EmailUnit(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
    new EmailAddressValidator().ValidateAndThrow(Address);
  }
}
