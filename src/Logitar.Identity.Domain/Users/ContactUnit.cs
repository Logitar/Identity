namespace Logitar.Identity.Domain.Users;

public abstract record ContactUnit : IContact
{
  public bool IsVerified { get; }

  protected ContactUnit(bool isVerified = false)
  {
    IsVerified = isVerified;
  }
}
