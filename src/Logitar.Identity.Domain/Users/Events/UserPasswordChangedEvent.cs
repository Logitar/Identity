using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent : UserPasswordEvent
{
  public UserPasswordChangedEvent(ActorId actorId, Password password) : base(actorId, password)
  {
  }
}
