namespace Logitar.Identity.Domain.Users;

public abstract record ContactUnit
{
  public bool IsVerified { get; }

  public ContactUnit(bool isVerified = false)
  {
    IsVerified = isVerified;
  }
}
