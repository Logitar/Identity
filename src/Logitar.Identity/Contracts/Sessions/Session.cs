using Logitar.Identity.Contracts.Users;

namespace Logitar.Identity.Contracts.Sessions;

public class Session : Aggregate
{
  public Guid Id { get; set; }

  public bool IsActive { get; set; }

  public string? RefreshToken { get; set; }

  public User User { get; set; } = new();
}
