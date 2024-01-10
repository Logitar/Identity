using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordUpdatedEvent : UserPasswordEvent
{
  public UserPasswordUpdatedEvent(ActorId actorId, Password password) : base(actorId, password)
  {
  }
}
