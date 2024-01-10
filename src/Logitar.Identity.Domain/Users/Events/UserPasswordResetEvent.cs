using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordResetEvent : UserPasswordEvent
{
  public UserPasswordResetEvent(ActorId actorId, Password password) : base(actorId, password)
  {
  }
}
