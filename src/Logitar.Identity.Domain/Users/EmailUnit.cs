using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record EmailUnit : ContactUnit, IEmail
{
  public const int MaximumLength = byte.MaxValue;

  public string Address { get; }

  public EmailUnit(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
    new EmailValidator().ValidateAndThrow(this);
  }
}
