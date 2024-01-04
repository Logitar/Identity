using Logitar.Identity.Contracts.Sessions;

namespace Logitar.Identity.Contracts.Users;

public class User : Aggregate
{
  public List<Session> Sessions { get; set; } = [];
}
